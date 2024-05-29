using RedditDataViewerAPI.Interfaces;
using RedditDataViewerAPI.Models;
using RedditDataViewerAPI.Providers;
using System.Diagnostics.Metrics;

namespace RedditDataViewerAPI.Services
{
    public class RedditWatchBackgroundService : BackgroundService
    {
        private readonly IRedditService _redditservice;
        private readonly ILogger<RedditService> _logger;
        public RedditWatchBackgroundService(IRedditService redditService, ILogger<RedditService> Logger)
        {
            _redditservice = redditService;
            _logger = Logger;
        }
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (true)
            {
                _logger.LogInformation($"Background Services Started at {GlobalData.ServiceStartTime}");
                if (GlobalData.FirstCall || (DateTime.UtcNow.Subtract(GlobalData.LastCallTime).TotalSeconds > 1))
                {
                    GlobalData.LastCallTime = DateTime.UtcNow;
                    await Task.Run(() => _redditservice.RedditServiceForAllPages());
                    _logger.LogInformation($"Completed Gathering Metrics at {DateTime.Now}");
                    GlobalData.CreatedDate = DateTime.UtcNow;
                    GlobalData.FirstCall = false;
                }
            }
        }
    }
}
