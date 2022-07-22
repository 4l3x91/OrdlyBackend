using OrdlyBackend.Interfaces;
using OrdlyBackend.Models;
using OrdlyBackend.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace OrdlyBackend.Services
{
    public class WordService : IWordService
    {
        private readonly OrdlyContext _context;
        public WordService(OrdlyContext context)
        {
            _context = context;
        }

        public OrdlyContext Context { get; }

        public async Task<List<Word>> GetAllWordsAsync()
        {
            return await _context.Words.ToListAsync();
        }

        public async Task<Word> GetRandomWordAsync()
        {
            int amountOfWords = _context.Words.Count();
            Random random = new Random();
            int randomId = random.Next(1, amountOfWords);
            return await _context.Words.FindAsync(randomId);
        }
    }
}
