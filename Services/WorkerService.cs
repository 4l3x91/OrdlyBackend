using OrdlyBackend.Interfaces;
using OrdlyBackend.Models;

namespace OrdlyBackend.Services;

public class WorkerService : BackgroundService
{
    private DateTime _DateLastRun;
    private bool inProgress = false;

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
            if(!inProgress) await SetDailyWordAsync();
            await Task.Delay(100);
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
                while(choosenWord == null)
                {
                   choosenWord = await GetWordAsync(wordService, dailyWordService);
                }
                var dailyWord = new DailyWord() { WordId = choosenWord.WordId, Date = DateTime.Now };

                dailyWordService.AddNewDailyWord(dailyWord);
            }

                _DateLastRun = DateTime.Now.Date;
                inProgress = false;
        }
    }

    private async Task<Word> GetWordAsync(IWordService wordService, IDailyWordService dailyWordService)
    {
        var randomWord = await wordService.GetRandomWordAsync();
        var latestWords = await dailyWordService.GetLatestDailys();
        if(latestWords.Any(x=> x.WordId == randomWord.WordId))
        {
            return null;
        }

        return randomWord;

    }
}