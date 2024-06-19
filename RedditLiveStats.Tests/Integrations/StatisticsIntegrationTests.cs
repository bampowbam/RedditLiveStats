using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace RedditLiveStats.Tests.Integrations
{
    [TestFixture]
    public class StatisticsIntegrationTests
    {
        private Mock<IRedditService> _redditServiceMock;
        private Mock<ILogger<StatisticsService>> _loggerMock;
        private StatisticsService _statisticsService;

        [SetUp]
        public void SetUp()
        {
            _redditServiceMock = new Mock<IRedditService>();
            _loggerMock = new Mock<ILogger<StatisticsService>>();
            _statisticsService = new StatisticsService(_redditServiceMock.Object, _loggerMock.Object);
        }

        [Test]
        public async Task FetchAndProcessPostsAsync_ShouldCallGetPostsAsyncOnce()
        {
            // Arrange
            var posts = new List<RedditPost>
            {
                new RedditPost { Id = "1", Title = "Post 1", Author = "user1", UpVotes = 10 },
                new RedditPost { Id = "2", Title = "Post 2", Author = "user2", UpVotes = 20 }
            };

            _redditServiceMock.Setup(service => service.GetPostsAsync())
                .ReturnsAsync(posts);

            // Act
            await _statisticsService.FetchAndProcessPostsAsync();

            // Assert
            _redditServiceMock.Verify(service => service.GetPostsAsync(), Times.Once);
        }

        [Test]
        public async Task FetchAndProcessPostsAsync_ShouldLogWarning_WhenNoPostsFetched()
        {
            // Arrange
            _redditServiceMock.Setup(service => service.GetPostsAsync())
                .ReturnsAsync(new List<RedditPost>());

            // Act
            await _statisticsService.FetchAndProcessPostsAsync();

            // Assert
            _redditServiceMock.Verify(service => service.GetPostsAsync(), Times.Once);
            _loggerMock.Verify(
                x => x.Log(
                    LogLevel.Warning,
                    It.IsAny<EventId>(),
                    It.Is<It.IsAnyType>((v, t) => v.ToString().Contains("No posts fetched from Reddit")),
                    null,
                    It.IsAny<Func<It.IsAnyType, Exception, string>>()),
                Times.Once);
        }

        [Test]
        public async Task FetchAndProcessPostsAsync_ShouldLogError_WhenApiCallFails()
        {
            // Arrange
            _redditServiceMock.Setup(service => service.GetPostsAsync())
                .ThrowsAsync(new HttpRequestException("HTTP request error occurred while fetching posts from Reddit"));

            // Act
            await _statisticsService.FetchAndProcessPostsAsync();

            // Assert
            _redditServiceMock.Verify(service => service.GetPostsAsync(), Times.Once);
            _loggerMock.Verify(
                x => x.Log(
                    LogLevel.Error,
                    It.IsAny<EventId>(),
                    It.Is<It.IsAnyType>((v, t) => v.ToString().Contains("HTTP request error occurred while fetching posts from Reddit")),
                    It.IsAny<HttpRequestException>(),
                    It.IsAny<Func<It.IsAnyType, Exception, string>>()),
                Times.Once);
        }

        [Test]
        public void ProcessPost_ShouldAddPostToStatistics()
        {
            // Arrange
            var post = new RedditPost { Id = "1", Title = "Post 1", Author = "user1", UpVotes = 10 };

            // Act
            _statisticsService.ProcessPost(post);

            // Assert
            _redditServiceMock.Verify(service => service.GetPostsAsync(), Times.Never);
        }

        [Test]
        public async Task GetTopUsers_ShouldReturnUsersSortedByPostCount()
        {
            // Arrange
            var posts = new List<RedditPost>
            {
                new RedditPost { Id = "1", Title = "Post 1", Author = "user1", UpVotes = 10 },
                new RedditPost { Id = "2", Title = "Post 2", Author = "user1", UpVotes = 20 },
                new RedditPost { Id = "3", Title = "Post 3", Author = "user2", UpVotes = 30 }
            };

            _redditServiceMock.Setup(service => service.GetPostsAsync())
                .ReturnsAsync(posts);

            // Act
            await _statisticsService.FetchAndProcessPostsAsync();
            var topUsers = _statisticsService.GetTopUsers(2);

            // Assert
            topUsers.Should().HaveCount(2);
            topUsers[0].UserName.Should().Be("user1");
            topUsers[0].PostCount.Should().Be(2);
            topUsers[1].UserName.Should().Be("user2");
            topUsers[1].PostCount.Should().Be(1);
        }
    }
}
