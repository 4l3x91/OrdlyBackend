using Microsoft.EntityFrameworkCore;
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

        public async Task<bool> AddNewDailyWordAsync(DailyWord newDaily)
        {
            await _context.DailyWords.AddAsync(newDaily);
            return await _context.SaveChangesAsync() > 0;
        }

        public Task<DailyWord> GetLatestDailyAsync()
        {
            return _context.DailyWords.LastAsync();
        }

        public async Task<List<DailyWord>> GetLatestDailysAsync()
        {
            return await _context.DailyWords.OrderByDescending(x => x.DailyWordId).Take(30).ToListAsync();
        }

    }
}
