using Microsoft.EntityFrameworkCore;
using OrdlyBackend.Infrastructure.Data;
using OrdlyBackend.Interfaces;
using OrdlyBackend.Models;

namespace OrdlyBackend.Services
{
    public class WordService : IWordService
    {
        private IRepository<Word> _wordRepository;

        public WordService(IRepository<Word> wordRepository)
        {
            _wordRepository = wordRepository;
        }

        public async Task<List<Word>> GetAllWordsAsync()
        {
            return await _wordRepository.GetAllAsync();
        }

        public async Task<Word> GetRandomWordAsync()
        {
            Random random = new Random();
            List<Word> allWords = await _wordRepository.GetAllAsync();

            int amountOfWords = allWords.Count;
            int randomId = random.Next(1, amountOfWords);

            return await _wordRepository.GetByIdAsync(randomId);
        }
    }
}
