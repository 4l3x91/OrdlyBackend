using OrdlyBackend.Infrastructure.Data;
using OrdlyBackend.Interfaces;
using OrdlyBackend.Models;

namespace OrdlyBackend.Services
{
    public class GuessService : IGuessService
    {
        private IRepository<Guess> _guessRepository;

        public GuessService(IRepository<Guess> guessRepository)
        {
            _guessRepository = guessRepository;
        }

        public async Task<List<Guess>> GetAllGuessesAsync()
        {
            return await _guessRepository.GetAllAsync();
        }

        public async Task<bool> AddGuessAsync(Guess guess)
        {
            return await _guessRepository.AddAsync(guess) != null;
        }
    }
}
