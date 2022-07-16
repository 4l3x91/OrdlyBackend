using OrdlyBackend.Models;

namespace OrdlyBackend.Interfaces
{
    public interface IDailyWordService
    {
        Task<bool> AddNewDailyWord(DailyWord newDaily);
        Task<List<DailyWord>> GetLatestDailys();
    }
}
