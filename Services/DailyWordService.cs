using OrdlyBackend.Interfaces;
using OrdlyBackend.Models;

namespace OrdlyBackend.Services
{
    public class DailyWordService : IDailyWordService
    {
        //private const int defaultAmountOfPrev = 30;
        IRepository<DailyWord> _dailyWordRepo;

        public DailyWordService(IRepository<DailyWord> dailyWordRepo)
        {
            _dailyWordRepo = dailyWordRepo;
        }

        public async Task<bool> AddNewDailyWordAsync(DailyWord newDaily)
        {
            return await _dailyWordRepo.AddAsync(newDaily) != null;
        }

        public async Task<DailyWord> GetDailyByIdAsync(int id)
        {
            //TODO Lägg till en metod för att hämta alla Dailies
            return await GetLatestDailiesAsync(100).ContinueWith(task => task.Result.FirstOrDefault(x=> x.Id == id));
        }

        public async Task<DailyWord> GetLatestDailyAsync()
        {
            return await _dailyWordRepo.GetLastAsync();
        }

        public async Task<List<DailyWord>> GetLatestDailiesAsync(int amountOfPrev)
        {
            var dailyWords = await _dailyWordRepo.GetAllAsync();
            return dailyWords.OrderByDescending(x => x.Id).Take(amountOfPrev).ToList();
        }

    }
}
