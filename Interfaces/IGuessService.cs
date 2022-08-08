using OrdlyBackend.Models;

namespace OrdlyBackend.Interfaces
{
    public interface IGuessService
    {
        Task<bool> AddGuessAsync(Guess guess);
        Task<List<Guess>> GetAllGuessesAsync();
    }
}
