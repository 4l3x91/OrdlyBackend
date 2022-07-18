using OrdlyBackend.Models;

namespace OrdlyBackend.Interfaces
{
    public interface IDailyWordService
    {
        Task<bool> AddNewDailyWordAsync(DailyWord newDaily);
        Task<List<DailyWord>> GetLatestDailysAsync();
    }
}
