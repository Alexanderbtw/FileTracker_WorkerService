namespace FileTracker_WorkerService
{
    public class FileObserverService : BackgroundService
    {
        private readonly ILogger<FileObserverService> _logger;

        public FileObserverService(ILogger<FileObserverService> logger)
        {
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var observable_path = Path.Combine(Directory.GetCurrentDirectory(), "ObservableFolder");
            if (!Directory.Exists(observable_path))
                Directory.CreateDirectory(observable_path);
            using var watcher = new FileSystemWatcher(observable_path);

            watcher.Changed += (o, e) => _logger.LogDebug($"{DateTime.Now} Changed: {e.FullPath}\n");
            watcher.Created += (o, e) => _logger.LogInformation($"{DateTime.Now} Created: {e.FullPath}\n");
            watcher.Deleted += (o, e) => _logger.LogWarning($"{DateTime.Now} Deleted: {e.FullPath}\n");
            watcher.Renamed += (o, e) => _logger.LogTrace($"{DateTime.Now} Renamed: {e.OldFullPath} to {e.FullPath}\n");
            watcher.Error += (o, e) => _logger.LogError($"{DateTime.Now} Error: {e.GetException().Message}\n"); 

            watcher.IncludeSubdirectories = true;
            watcher.EnableRaisingEvents = true;

            while (!stoppingToken.IsCancellationRequested)
            {
                await Task.Delay(1000, stoppingToken);
            }
            await Task.CompletedTask;
        }
    }
}