using System.Collections.Generic;
using System.Threading.Tasks;

/// <summary>
/// Interface for managing Reddit statistics.
/// </summary>
public interface IStatisticsService
{
    /// <summary>
    /// Fetches posts from Reddit and processes them.
    /// </summary>
    Task FetchAndProcessPostsAsync();

    /// <summary>
    /// Processes a single Reddit post.
    /// </summary>
    /// <param name="post">The post to process.</param>
    void ProcessPost(RedditPost post);

    /// <summary>
    /// Gets the top posts based on upvotes.
    /// </summary>
    /// <param name="count">The number of top posts to retrieve.</param>
    /// <returns>A list of top posts.</returns>
    List<RedditPost> GetTopPosts(int count);

    /// <summary>
    /// Gets the top users based on post count.
    /// </summary>
    /// <param name="count">The number of top users to retrieve.</param>
    /// <returns>A list of top users.</returns>
    List<UserStatistics> GetTopUsers(int count);
}
