using OrdlyBackend.Models;

namespace OrdlyBackend.Interfaces
{
    public interface IWordService
    {
        Task<Word> GetRandomWordAsync();
        Task<List<Word>> GetAllWordsAsync();
        Task<Word> GetWordByIdAsync(int id);
    }
}