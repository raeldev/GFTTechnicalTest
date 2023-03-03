using TaskManager.Domain.Services;

namespace TaskManager.Worker
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;
        private readonly IQueueService _queueService;

        public Worker(ILogger<Worker> logger, IQueueService queueService)
        {
            _logger = logger;
            _queueService = queueService;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);
            await _queueService.StartProcess(stoppingToken);
        }
    }
}