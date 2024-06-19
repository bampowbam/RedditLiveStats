using FluentAssertions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using System.IO;
using System.IO.Compression;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace RedditLiveStats.Tests.Integrations
{
    [TestFixture]
    public class RedditIntegrationTests
    {
        private Mock<IRestClient> _restClientMock;
        private Mock<IConfiguration> _configurationMock;
        private Mock<ILogger<RedditService>> _loggerMock;
        private IRedditService _redditService;

        [SetUp]
        public void SetUp()
        {
            _restClientMock = new Mock<IRestClient>();
            _configurationMock = new Mock<IConfiguration>();
            _loggerMock = new Mock<ILogger<RedditService>>();

            _configurationMock.Setup(config => config["Reddit:AccessToken"]).Returns("fakeAccessToken");

            _redditService = new RedditService(_restClientMock.Object, _configurationMock.Object, _loggerMock.Object);
        }

        [Test]
        public async Task GetPostsAsync_ShouldCallRestClientSendAsyncOnce()
        {
            // Arrange
            var responseContent = "{\"data\":{\"children\":[{\"data\":{\"id\":\"1\",\"title\":\"Post 1\",\"author\":\"user1\",\"ups\":10}},{\"data\":{\"id\":\"2\",\"title\":\"Post 2\",\"author\":\"user2\",\"ups\":20}}]}}";
            var httpResponse = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = CompressionHelper.CreateGZipContent(responseContent)
            };

            _restClientMock.Setup(client => client.SendAsync(It.IsAny<HttpRequestMessage>()))
                .ReturnsAsync(httpResponse);

            // Act
            var posts = await _redditService.GetPostsAsync();

            // Assert
            _restClientMock.Verify(client => client.SendAsync(It.IsAny<HttpRequestMessage>()), Times.Once);
        }

        [Test]
        public async Task GetPostsAsync_ShouldReturnEmptyList_WhenApiCallFails()
        {
            // Arrange
            var httpResponse = new HttpResponseMessage(HttpStatusCode.BadRequest);
            _restClientMock.Setup(client => client.SendAsync(It.IsAny<HttpRequestMessage>()))
                .ReturnsAsync(httpResponse);

            // Act
            var posts = await _redditService.GetPostsAsync();

            // Assert
            _restClientMock.Verify(client => client.SendAsync(It.IsAny<HttpRequestMessage>()), Times.Once);
            posts.Should().BeEmpty();
        }
    }

    public static class CompressionHelper
    {
        public static HttpContent CreateGZipContent(string content)
        {
            var contentBytes = Encoding.UTF8.GetBytes(content);
            var memoryStream = new MemoryStream();
            using (var gzipStream = new GZipStream(memoryStream, CompressionMode.Compress, true))
            {
                gzipStream.Write(contentBytes, 0, contentBytes.Length);
            }
            memoryStream.Seek(0, SeekOrigin.Begin);
            var compressedContent = new ByteArrayContent(memoryStream.ToArray());
            compressedContent.Headers.ContentEncoding.Add("gzip");
            compressedContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            return compressedContent;
        }
    }
}
