using OrdlyBackend.Infrastructure.Data;
using OrdlyBackend.Interfaces;
using OrdlyBackend.Models;
using System.Linq;

namespace OrdlyBackend.Services
{
    public class DailyWordService : IDailyWordService
    {
        OrdlyContext _context;
        public DailyWordService(OrdlyContext context)
        {
            _context = context;
        }

        public async Task<bool> AddNewDailyWord(DailyWord newDaily)
        {
            _context.DailyWords.AddAsync(newDaily);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<List<DailyWord>> GetLatestDailys()
        {
            return await Task.Run(()=> _context.DailyWords.Take(30).ToList());
        }

    }
}
