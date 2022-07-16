using OrdlyBackend.Models;

namespace OrdlyBackend.Interfaces
{
    public interface IWordService
    {
        Task<Word> GetRandomWordAsync();
    }
}