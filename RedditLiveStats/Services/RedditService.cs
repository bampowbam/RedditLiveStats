using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;
using System.IO.Compression;

/// <summary>
/// Provides methods for interacting with the Reddit API.
/// </summary>
public class RedditService : IRedditService
{
    private readonly IRestClient _restClient;
    private readonly string _accessToken;
    private readonly ILogger<RedditService> _logger;

    public RedditService(IRestClient restClient, IConfiguration configuration, ILogger<RedditService> logger)
    {
        _restClient = restClient;
        _accessToken = configuration["Reddit:AccessToken"];
        _logger = logger;
    }

    /// <inheritdoc />
    public async Task<List<RedditPost>> GetPostsAsync()
    {
        _logger.LogInformation("Fetching posts from Reddit API");

        var request = new HttpRequestMessage(HttpMethod.Get, "https://oauth.reddit.com/r/wallstreetbets/new?limit=10000");
        request.Headers.Add("Authorization", $"Bearer {_accessToken}");
        request.Headers.Add("User-Agent", "PostmanRuntime/7.39.0");
        request.Headers.Add("Accept", "*/*");
        request.Headers.Add("Accept-Encoding", "gzip, deflate, br");
        request.Headers.Add("Connection", "keep-alive");

        HttpResponseMessage response;
        try
        {
            response = await _restClient.SendAsync(request);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while sending request to Reddit API");
            return new List<RedditPost>();
        }

        if (!response.IsSuccessStatusCode)
        {
            _logger.LogError("Failed to fetch posts: {StatusCode}", response.StatusCode);
            return new List<RedditPost>();
        }

        string content;
        if (response.Content.Headers.ContentEncoding.Contains("gzip"))
        {
            using (var decompressedStream = new GZipStream(await response.Content.ReadAsStreamAsync(), CompressionMode.Decompress))
            using (var streamReader = new StreamReader(decompressedStream))
            {
                content = await streamReader.ReadToEndAsync();
            }
        }
        else
        {
            content = await response.Content.ReadAsStringAsync();
        }

        _logger.LogInformation("Response content: {Content}", content);

        var posts = JObject.Parse(content)["data"]["children"]
            .Select(p => new RedditPost
            {
                Id = p["data"]["id"].ToString(),
                Title = p["data"]["title"].ToString(),
                Author = p["data"]["author"].ToString(),
                UpVotes = (int)p["data"]["ups"]
            }).ToList();

        _logger.LogInformation("Fetched {Count} posts", posts.Count);
        return posts;
    }
}
