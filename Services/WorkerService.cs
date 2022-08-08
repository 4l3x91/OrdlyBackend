using OrdlyBackend.Interfaces;
using OrdlyBackend.Models;
using OrdlyBackend.Utilities;

namespace OrdlyBackend.Services;

public class WorkerService : BackgroundService
{
    private DateTime? _DateLastRun;
    private bool inProgress = false;
    private ILogger<WorkerService> _logger;
    private OrdlySettings _settings;

    private readonly IServiceScopeFactory _serviceScopeFactory;
    public WorkerService(IServiceScopeFactory serviceScopeFactory, ILogger<WorkerService> logger, OrdlySettings settings)
    {
        _logger = logger;
        _settings = settings;
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
            if (!inProgress) await SetDailyWordAsync();
            await Task.Delay(5000);
        }
    }

    public async Task SetDailyWordAsync()
    {
        inProgress = true;
        if (_DateLastRun < DateTime.Now.Date || _settings.ForceNewDaily)
        {
            using (var scope = _serviceScopeFactory.CreateScope())
            {
                var dailyWordService = scope.ServiceProvider.GetRequiredService<IDailyWordService>();
                var wordService = scope.ServiceProvider.GetRequiredService<IWordService>();

                var choosenWord = await GetWordAsync(dailyWordService, wordService);

                var dailyWord = new DailyWord()
                {
                    WordId = choosenWord.Id,
                    Date = DateTime.Now.Date
                };

                await dailyWordService.AddNewDailyWordAsync(dailyWord);
            }

            _DateLastRun = DateTime.Now.Date;
            _logger.LogInformation("WorkerService successfully added a new DailyWord at " + DateTime.Now);
            _settings.ForceNewDaily = false;
        }
        inProgress = false;
    }

    private async Task<Word> GetWordAsync(IDailyWordService dailyWordService, IWordService wordService)
    {
        Word choosenWord = null;

        var wordList = await wordService.GetAllWordsAsync();
        while (choosenWord == null)
        {
            if (_settings.WordCategory == "all") choosenWord = await GetAnyWordAsync(wordService, dailyWordService);
            else choosenWord = await GetWordByCategoryAsync(wordService, dailyWordService);
        }
        return choosenWord;
    }

    private async Task InitiateLastRun()
    {
        using (var scope = _serviceScopeFactory.CreateScope())
        {
            var dailyWordService = scope.ServiceProvider.GetRequiredService<IDailyWordService>();
            var lastDailys = await dailyWordService.GetLatestDailiesAsync();

            if (lastDailys.Count != 0)
            {
                var lastDaily = lastDailys.First();
                _DateLastRun = lastDaily.Date.Date;
            }
            else
            {
                _DateLastRun = DateTime.Now.Date.AddDays(-1).Date;
            }
        }
    }

    private async Task<Word> GetAnyWordAsync(IWordService wordService, IDailyWordService dailyWordService)
    {
        var randomWord = await wordService.GetRandomWordAsync();
        return await isDuplicatedWordAsync(dailyWordService, randomWord.Id) ? null : randomWord;
    }

    private async Task<Word> GetWordByCategoryAsync(IWordService wordService, IDailyWordService dailyWordService)
    {
        var randomWord = await wordService.GetRandomWordByCategoryAsync(_settings.WordCategory);
        return await isDuplicatedWordAsync(dailyWordService, randomWord.Id) ? null : randomWord;
    }

    private async Task<bool> isDuplicatedWordAsync(IDailyWordService dailyWordService, int id)
    {
        var latestWords = await dailyWordService.GetLatestDailiesAsync();
        if (latestWords.Any(x => x.WordId == id))
        {
            return true;
        }
        return false;
    }
}
