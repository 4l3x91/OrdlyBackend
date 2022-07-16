using Microsoft.Extensions.Logging;
using OrdlyBackend.Interfaces;
using OrdlyBackend.Models;

namespace OrdlyBackend.Services;

public class WorkerService : BackgroundService
{
    private DateTime? _DateLastRun;
    private bool inProgress = false;
    private ILogger<WorkerService> _logger;

    private readonly IServiceScopeFactory _serviceScopeFactory;
    public WorkerService(IServiceScopeFactory serviceScopeFactory, ILogger<WorkerService> logger)
    {
        _logger = logger;
        _serviceScopeFactory = serviceScopeFactory;
        _DateLastRun = null;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        if (!_DateLastRun.HasValue)
        {
            await InitiateLastRun();
        }

        while (!stoppingToken.IsCancellationRequested)
        {
            if(!inProgress) await SetDailyWordAsync();
            await Task.Delay(60000);
        }
    }

    public async Task SetDailyWordAsync()
    {
        inProgress = true;

        if (_DateLastRun < DateTime.Now.Date)
        {
            using (var scope = _serviceScopeFactory.CreateScope())
            {
                var dailyWordService = scope.ServiceProvider.GetRequiredService<IDailyWordService>();
                var wordService = scope.ServiceProvider.GetRequiredService<IWordService>();

                Word choosenWord = null;
                while (choosenWord == null)
                {
                    choosenWord = await GetWordAsync(wordService, dailyWordService);
                }
                var dailyWord = new DailyWord() { WordId = choosenWord.WordId, Date = DateTime.Now.Date };

                await dailyWordService.AddNewDailyWordAsync(dailyWord);
            }
            _DateLastRun = DateTime.Now.Date;
            _logger.LogInformation("WorkerService succeded at " + DateTime.Now);
        }
              inProgress = false;
    }

    private async Task InitiateLastRun()
    {
        using (var scope = _serviceScopeFactory.CreateScope())
        {
            var dailyWordService = scope.ServiceProvider.GetRequiredService<IDailyWordService>();
            var lastDailys = await dailyWordService.GetLatestDailysAsync();
            var lastDaily = lastDailys.First();
            _DateLastRun = lastDaily.Date.Date;
        }
    }

    private async Task<Word> GetWordAsync(IWordService wordService, IDailyWordService dailyWordService)
    {
        var randomWord = await wordService.GetRandomWordAsync();
        var latestWords = await dailyWordService.GetLatestDailysAsync();
        if(latestWords.Any(x=> x.WordId == randomWord.WordId))
        {
            return null;
        }
        return randomWord;
    }
}
