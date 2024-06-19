/// <summary>
/// Represents statistics for a Reddit user.
/// </summary>
public class UserStatistics
{
    /// <summary>
    /// Gets or sets the username of the user.
    /// </summary>
    public string UserName { get; set; }

    /// <summary>
    /// Gets or sets the total number of posts by the user.
    /// </summary>
    public int PostCount { get; set; }

    /// <summary>
    /// Gets or sets the total number of upvotes received by the user's posts.
    /// </summary>
    public int UpVotes { get; set; }

    /// <summary>
    /// Gets or sets the List of user's Reddit Posts.
    /// </summary>
    public List<RedditPost> Posts { get; set; } = new List<RedditPost>();
}
