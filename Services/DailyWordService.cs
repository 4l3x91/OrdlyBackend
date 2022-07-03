using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;

namespace ApplicationCore.Services;

public class WorkerService : BackgroundService
{
    private DateTime _DateLastRun;
    private readonly IServiceScopeFactory _serviceScopeFactory;
    public WorkerService(IServiceScopeFactory serviceScopeFactory)
    {
        _serviceScopeFactory = serviceScopeFactory;
        _DateLastRun = DateTime.Now.Date;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            await SetDailyWord();
            await Task.Delay(100);
        }
    }

    public async Task SetDailyWord()
    {
        if (_DateLastRun < DateTime.Now.Date)
        {
            Console.WriteLine(DateTime.Now.Date);
            Console.WriteLine("\n");
            Console.WriteLine(_DateLastRun);
            _DateLastRun = DateTime.Now.Date;
            Console.WriteLine("\n");
            Console.WriteLine(_DateLastRun);
        }
    }
}