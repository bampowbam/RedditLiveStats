using System.Threading.Tasks;

/// <summary>
/// Interface for Reddit service to fetch live stream posts.
/// </summary>
public interface IRedditService
{
    /// <summary>
    /// Gets live stream posts from a specific stream.
    /// </summary>
    /// <returns>The JSON content about posts.</returns>
    Task<List<RedditPost>> GetPostsAsync();
}
