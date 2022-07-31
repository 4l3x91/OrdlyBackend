using Microsoft.EntityFrameworkCore;
using OrdlyBackend.DTOs;
using OrdlyBackend.DTOs.v2;
using OrdlyBackend.Infrastructure.Data;
using OrdlyBackend.Interfaces;
using OrdlyBackend.Models;

namespace OrdlyBackend.Services
{
    public class UserGameService : IUserGameService
    {
        IRepository<UserGame> _userGameRepository;
        IRepository<Guess> _guessRepository;

        public UserGameService(IRepository<UserGame> userGameRepository, IRepository<Guess> guessRepository)
        {
            _userGameRepository = userGameRepository;
            _guessRepository = guessRepository;
        }

        //Skapa en GuessService och lägga där?
        public async Task<bool> AddGuessAsync(GuessRequest request, DailyWord daily, List<Word> allWords, GuessResponse2 guessResonse)
        {
            UserGame userGame = await GetUserGameAsync(request, daily, guessResonse);

            Guess guess = new()
            {
                UserGameId = userGame.Id,
                WordId = allWords.Find(word => word.Name == request.Guess).Id
            };

            return await _guessRepository.AddAsync(guess) != null;

        }

        public async Task<UserGame> GetUserGameByUserIdAsync(int userId)
        {
            return await _userGameRepository.GetByIdAsync(userId);
        }




        private async Task<UserGame> GetUserGameAsync(GuessRequest request, DailyWord daily, GuessResponse2 guessResonse)
        {
            var userGame = await GetUserGameByUserIdAsync(request.UserId);
            if (userGame == null)
            {
                userGame = await CreateNewUserGameAsync(request.UserId, daily.Id, guessResonse.isCompleted);
            }
            else if (guessResonse.isCompleted)
            {
                await UpdateUserGameStatusAsync(guessResonse.isCompleted, userGame);
            }

            return userGame;
        }

        private async Task UpdateUserGameStatusAsync(bool isCompleted, UserGame userGame)
        {
            userGame.isCompleted = isCompleted;
            await _userGameRepository.UpdateAsync(userGame);
        }

        private async Task<UserGame> CreateNewUserGameAsync(int userId, int dailyWordId, bool isCompleted)
        {
            UserGame newUG = new()
            {
                UserId = userId,
                DailyWordId = dailyWordId,
                isCompleted = isCompleted,
            };

            return await _userGameRepository.AddAsync(newUG);
        }
    }
}
