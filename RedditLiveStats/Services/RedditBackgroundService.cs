using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

/// <summary>
/// Background service to periodically fetch and process posts from Reddit.
/// </summary>
public class RedditBackgroundService : BackgroundService
{
    private readonly IStatisticsService _statisticsService;
    private readonly ILogger<RedditBackgroundService> _logger;

    /// <summary>
    /// Initializes a new instance of the <see cref="RedditBackgroundService"/> class.
    /// </summary>
    /// <param name="statisticsService">The statistics service used to fetch and process posts.</param>
    /// <param name="logger">The logger used to log information and errors.</param>
    public RedditBackgroundService(IStatisticsService statisticsService, ILogger<RedditBackgroundService> logger)
    {
        _statisticsService = statisticsService;
        _logger = logger;
    }

    /// <summary>
    /// Executes the background service.
    /// </summary>
    /// <param name="stoppingToken">A token that can be used to stop the background service.</param>
    /// <returns>A task that represents the background service execution.</returns>
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            _logger.LogInformation("Fetching and processing posts at: {time}", DateTimeOffset.Now);
            await _statisticsService.FetchAndProcessPostsAsync();
            _logger.LogInformation("Successfully fetched and processed posts.");
            await Task.Delay(TimeSpan.FromMinutes(1), stoppingToken);
        }
    }
}
