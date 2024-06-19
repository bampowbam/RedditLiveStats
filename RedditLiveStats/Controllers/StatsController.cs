using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

[ApiController]
[Route("[controller]")]
public class StatsController : ControllerBase
{
    private readonly IStatisticsService _statisticsService;
    private readonly ILogger<StatsController> _logger;

    public StatsController(IStatisticsService statisticsService, ILogger<StatsController> logger)
    {
        _statisticsService = statisticsService;
        _logger = logger;
    }

    /// <summary>
    /// Fetch and process Reddit posts.
    /// </summary>
    [HttpGet("fetch-and-process")]
    public async Task<IActionResult> FetchAndProcess()
    {
        _logger.LogInformation("Fetching and processing Reddit posts");
        await _statisticsService.FetchAndProcessPostsAsync();
        return Ok("Posts fetched and processed successfully.");
    }

    /// <summary>
    /// Get top posts.
    /// </summary>
    /// <param name="count">Number of top posts to retrieve.</param>
    [HttpGet("top-posts")]
    public async Task<IActionResult> GetTopPosts(int count)
    {
        _logger.LogInformation("Fetching and processing Reddit posts for top posts retrieval");
        await _statisticsService.FetchAndProcessPostsAsync();

        _logger.LogInformation("Getting top {Count} posts", count);
        var topPosts = _statisticsService.GetTopPosts(count);
        return Ok(topPosts);
    }

    /// <summary>
    /// Get top users.
    /// </summary>
    /// <param name="count">Number of top users to retrieve.</param>
    [HttpGet("top-users")]
    public async Task<IActionResult> GetTopUsers(int count)
    {
        _logger.LogInformation("Fetching and processing Reddit posts for top users retrieval");
        await _statisticsService.FetchAndProcessPostsAsync();

        _logger.LogInformation("Getting top {Count} users", count);
        var topUsers = _statisticsService.GetTopUsers(count);
        return Ok(topUsers);
    }
}
