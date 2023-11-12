namespace FileTracker_WorkerService
{
    public class Program
    {
        public static void Main(string[] args)
        {
            IHost host = Host.CreateDefaultBuilder(args)
                .ConfigureServices(services =>
                {
                    services.AddHostedService<FileObserverService>();
                })
                .ConfigureLogging(options => options.AddFile(Path.Combine(Directory.GetCurrentDirectory(), "Events.txt")))
                .Build();

            host.Run();
        }
    }
}