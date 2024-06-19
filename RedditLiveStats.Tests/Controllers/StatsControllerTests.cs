using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using System.Collections.Generic;
using System.Threading.Tasks;
using FluentAssertions;

namespace RedditLiveStats.Tests.Controllers
{
    [TestFixture]
    public class StatsControllerTests
    {
        private Mock<IStatisticsService> _statisticsServiceMock;
        private Mock<ILogger<StatsController>> _loggerMock;
        private StatsController _controller;

        [SetUp]
        public void SetUp()
        {
            _statisticsServiceMock = new Mock<IStatisticsService>();
            _loggerMock = new Mock<ILogger<StatsController>>();
            _controller = new StatsController(_statisticsServiceMock.Object, _loggerMock.Object);
        }

        [Test]
        public async Task FetchAndProcess_ShouldCallFetchAndProcessPostsAsync()
        {
            // Act
            var result = await _controller.FetchAndProcess();

            // Assert
            _statisticsServiceMock.Verify(s => s.FetchAndProcessPostsAsync(), Times.Once);
            result.Should().BeOfType<OkObjectResult>();
        }

        [Test]
        public async Task GetTopPosts_ShouldCallFetchAndProcessPostsAsync_AndReturnTopPosts()
        {
            // Arrange
            var topPosts = new List<RedditPost>
            {
                new RedditPost { Id = "1", Title = "Post 1", Author = "user1", UpVotes = 10 },
                new RedditPost { Id = "2", Title = "Post 2", Author = "user2", UpVotes = 20 }
            };

            _statisticsServiceMock.Setup(s => s.FetchAndProcessPostsAsync()).Returns(Task.CompletedTask);
            _statisticsServiceMock.Setup(s => s.GetTopPosts(It.IsAny<int>())).Returns(topPosts);

            // Act
            var result = await _controller.GetTopPosts(2);

            // Assert
            _statisticsServiceMock.Verify(s => s.FetchAndProcessPostsAsync(), Times.Once);
            _statisticsServiceMock.Verify(s => s.GetTopPosts(2), Times.Once);
            var okResult = result as OkObjectResult;
            okResult.Should().NotBeNull();
            okResult.Value.Should().BeEquivalentTo(topPosts);
        }

        [Test]
        public async Task GetTopUsers_ShouldCallFetchAndProcessPostsAsync_AndReturnTopUsers()
        {
            // Arrange
            var topUsers = new List<UserStatistics>
            {
                new UserStatistics { UserName = "user1", PostCount = 10, UpVotes = 100 },
                new UserStatistics { UserName = "user2", PostCount = 20, UpVotes = 200 }
            };

            _statisticsServiceMock.Setup(s => s.FetchAndProcessPostsAsync()).Returns(Task.CompletedTask);
            _statisticsServiceMock.Setup(s => s.GetTopUsers(It.IsAny<int>())).Returns(topUsers);

            // Act
            var result = await _controller.GetTopUsers(2);

            // Assert
            _statisticsServiceMock.Verify(s => s.FetchAndProcessPostsAsync(), Times.Once);
            _statisticsServiceMock.Verify(s => s.GetTopUsers(2), Times.Once);
            var okResult = result as OkObjectResult;
            okResult.Should().NotBeNull();
            okResult.Value.Should().BeEquivalentTo(topUsers);
        }
    }
}
