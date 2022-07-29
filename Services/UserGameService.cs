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
        OrdlyContext _context;

        public UserGameService(OrdlyContext context)
        {
            _context = context;
        }

        public async Task<bool> AddGuessAsync(GuessRequest request, DailyWord daily, List<Word> allWords, GuessResponse2 guessResonse)
        {
            UserGame userGame = await GetUserGameAsync(request, daily, guessResonse);

            Guess guess = new()
            {
                UserGameId = userGame.UserGameId,
                WordId = allWords.Find(word => word.Name == request.Guess).WordId
            };

            await _context.Guesses.AddAsync(guess);
            return await _context.SaveChangesAsync() > 0;

        }

        public async Task<UserGame> GetUserGameByUserIdAsync(int userId)
        {
            return await _context.UserGames.FirstOrDefaultAsync(x => x.UserId == userId);
        }




        private async Task<UserGame> GetUserGameAsync(GuessRequest request, DailyWord daily, GuessResponse2 guessResonse)
        {
            var userGame = await GetUserGameByUserIdAsync(request.UserId);
            if (userGame == null)
            {
                userGame = await CreateNewUserGameAsync(request.UserId, daily.DailyWordId, guessResonse.isCompleted);
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
            _context.Update(userGame);
            await _context.SaveChangesAsync();
        }

        private async Task<UserGame> CreateNewUserGameAsync(int userId, int dailyWordId, bool isCompleted)
        {
            UserGame newUG = new()
            {
                UserId = userId,
                DailyWordId = dailyWordId,
                isCompleted = isCompleted,
            };

            await _context.UserGames.AddAsync(newUG);
            await _context.SaveChangesAsync();
            return newUG;
        }
    }
}
