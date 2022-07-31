using Microsoft.EntityFrameworkCore;
using OrdlyBackend.Infrastructure.Data;
using OrdlyBackend.Interfaces;
using OrdlyBackend.Models;
using System.Linq;

namespace OrdlyBackend.Services
{
    public class DailyWordService : IDailyWordService
    {
        private const int amountOfPrev = 30;
        IRepository<DailyWord> _dailyWordRepo;

        public DailyWordService(IRepository<DailyWord> dailyWordRepo)
        {
            _dailyWordRepo = dailyWordRepo;
        }

        public async Task<bool> AddNewDailyWordAsync(DailyWord newDaily)
        {
            return await _dailyWordRepo.AddAsync(newDaily) != null;
        }

        public async Task<DailyWord> GetLatestDailyAsync()
        {
            return await _dailyWordRepo.GetLastAsync();
        }

        public async Task<List<DailyWord>> GetLatestDailysAsync()
        {
            var dailyWords = await _dailyWordRepo.GetAllAsync();
            return dailyWords.OrderByDescending(x => x.DailyWordId).Take(amountOfPrev).ToList();
        }

    }
}
