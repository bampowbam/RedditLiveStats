using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

/// <summary>
/// Provides methods for managing Reddit statistics.
/// </summary>
public class StatisticsService : IStatisticsService
{
    private readonly List<RedditPost> _posts = new List<RedditPost>();
    private readonly Dictionary<string, UserStatistics> _users = new Dictionary<string, UserStatistics>();
    private readonly ILogger<StatisticsService> _logger;
    private readonly IRedditService _redditService;

    public StatisticsService(IRedditService redditService, ILogger<StatisticsService> logger)
    {
        _redditService = redditService;
        _logger = logger;
    }

    /// <inheritdoc />
    public async Task FetchAndProcessPostsAsync()
    {
        _logger.LogInformation("Fetching posts from Reddit API");

        List<RedditPost> posts;
        try
        {
            posts = await _redditService.GetPostsAsync();
        }
        catch (HttpRequestException ex)
        {
            _logger.LogError(ex, "HTTP request error occurred while fetching posts from Reddit");
            return;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unexpected error occurred while fetching posts from Reddit");
            return;
        }

        if (posts == null || posts.Count == 0)
        {
            _logger.LogWarning("No posts fetched from Reddit");
            return;
        }

        var oneMonthAgo = DateTime.UtcNow.AddMonths(-1);
        var recentPosts = posts.Where(post => post.Timestamp >= oneMonthAgo).ToList();

        _logger.LogInformation("Processing fetched posts");
        foreach (var post in recentPosts)
        {
            ProcessPost(post);
        }
    }

    /// <inheritdoc />
    public void ProcessPost(RedditPost post)
    {
        _logger.LogInformation("Processing post: {Title} by {Author}", post.Title, post.Author);

        var existingPost = _posts.FirstOrDefault(p => p.Id == post.Id);
        if (existingPost == null)
        {
            _posts.Add(post);
        }
        else
        {
            existingPost.UpVotes = post.UpVotes;
            existingPost.Timestamp = post.Timestamp;
        }

        if (_users.ContainsKey(post.Author))
        {
            _users[post.Author].PostCount = _posts.Count(p => p.Author == post.Author);
            _users[post.Author].UpVotes = _posts.Where(p => p.Author == post.Author).Sum(p => p.UpVotes);
            _logger.LogInformation("Updated statistics for user: {User}. PostCount: {PostCount}, UpVotes: {UpVotes}",
                post.Author, _users[post.Author].PostCount, _users[post.Author].UpVotes);
        }
        else
        {
            _users[post.Author] = new UserStatistics
            {
                UserName = post.Author,
                PostCount = 1,
                UpVotes = post.UpVotes
            };
            _logger.LogInformation("Created statistics for new user: {User}. PostCount: {PostCount}, UpVotes: {UpVotes}",
                post.Author, 1, post.UpVotes);
        }
    }

    /// <inheritdoc />
    public List<RedditPost> GetTopPosts(int count)
    {
        var timeLimit = DateTime.UtcNow.AddMonths(-1);
        var topPosts = _posts.Where(p => p.Timestamp >= timeLimit).OrderByDescending(p => p.UpVotes).Take(count).ToList();
        _logger.LogInformation("Retrieved top {Count} posts", count);
        return topPosts;
    }

    /// <inheritdoc />
    public List<UserStatistics> GetTopUsers(int count)
    {
        var timeLimit = DateTime.UtcNow.AddMonths(-1);
        var recentPosts = _posts.Where(p => p.Timestamp >= timeLimit).ToList();

        foreach (var user in _users.Values)
        {
            user.PostCount = recentPosts.Count(p => p.Author == user.UserName);
            user.UpVotes = recentPosts.Where(p => p.Author == user.UserName).Sum(p => p.UpVotes);
        }

        var topUsers = _users.Values.OrderByDescending(u => u.PostCount).ThenByDescending(u => u.UpVotes).Take(count).ToList();
        _logger.LogInformation("Retrieved top {Count} users", count);
        return topUsers;
    }
}
