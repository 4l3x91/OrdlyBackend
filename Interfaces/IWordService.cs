using OrdlyBackend.Models;

namespace OrdlyBackend.Interfaces
{
    public interface IWordService
    {
        Task<Word> GetRandomWordAsync();
        Task<Word> GetRandomWordByCategoryAsync(string category);
        Task<List<Word>> GetAllWordsAsync();
        Task<Word> GetWordByIdAsync(int id);
        Task<List<Word>> GetWordsByCategoryAsync(string category);
    }
}