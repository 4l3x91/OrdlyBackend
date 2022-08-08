using OrdlyBackend.Models;

namespace OrdlyBackend.Interfaces
{
    public interface IDailyWordService
    {
        private const int defaultAmountOfPrev = 30;
        Task<bool> AddNewDailyWordAsync(DailyWord newDaily);
        Task<DailyWord> GetDailyByIdAsync(int id);
        Task<List<DailyWord>> GetLatestDailiesAsync(int amountOfPrev = defaultAmountOfPrev);
        Task<DailyWord> GetLatestDailyAsync();
    }
}
