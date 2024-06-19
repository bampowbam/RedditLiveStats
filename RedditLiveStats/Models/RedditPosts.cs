
/// <summary>
/// Represents a Reddit post.
/// </summary>
public class RedditPost
{
    /// <summary>
    /// Gets or sets the ID of the post.
    /// </summary>
    public string Id { get; set; }

    /// <summary>
    /// Gets or sets the title of the post.
    /// </summary>
    public string Title { get; set; }

    /// <summary>
    /// Gets or sets the author of the post.
    /// </summary>
    public string Author { get; set; }

    /// <summary>
    /// Gets or sets the number of upvotes for the post.
    /// </summary>
    public int UpVotes { get; set; }

    /// <summary>
    /// Gets or sets the timestamp for the post.
    /// </summary>
    public DateTime Timestamp { get; set; } = DateTime.UtcNow;

    /// <summary>
    /// Gets or sets the Created time and date for the post.
    /// </summary>
    public DateTime CreatedUtc { get; set; }
}
