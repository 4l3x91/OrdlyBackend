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
            _DateLastRun = DateTime.Now.Date;
            await SetDailyWord();
        }
    }
}