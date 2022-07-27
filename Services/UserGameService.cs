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
            //Skapa om det inte finns ett userGame annars hämtar userGameId
            var userGame = _context.UserGames.FirstOrDefault(x => x.UserId == request.UserId);
            if (userGame == null)
            {
                UserGame newUG = new()
                {
                UserId = request.UserId,
                DailyWordId = daily.DailyWordId,
                isCompleted = guessResonse.isCompleted,
                };
               
                userGame = await _context.UserGames.AddAsync(newUG);
            }

            Guess guess = new()
            {
                UserGameId = userGame.UserGameId,
                WordId = allWords.Find(word => word.Name == request.Guess).WordId
            };

            await _context.Guesses.AddAsync(guess);
            return await _context.SaveChangesAsync() > 0;

        }
    }
}
