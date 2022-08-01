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



        public async Task<Word> GetRandomWordByCategoryAsync(string category)
        {
            Random random = new Random();
            List<Word> allWords = await GetWordsByCategoryAsync(category);

            int amountOfWords = allWords.Count;
            int randomNr = random.Next(0, amountOfWords-1);

            return allWords[randomNr];
        }

        public async Task<Word> GetWordByIdAsync(int id)
        {
            Word word = await _wordRepository.GetByIdAsync(id);
            return word;
        }

        public async Task<List<Word>> GetWordsByCategoryAsync(string category)
        {
            List<Word> words = await _wordRepository.GetAllAsync();
            return words.Where(x => x.Category == category).ToList();
        }
    }
}
