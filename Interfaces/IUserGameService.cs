using OrdlyBackend.DTOs.v1;
using OrdlyBackend.Models;

namespace OrdlyBackend.Interfaces;
public interface IUserGameService
{
    Task<bool> AddGuessAsync(GuessRequest request, DailyWord daily, List<Word> allWords, GuessResponse guessResonse);
    Task<UserGame> GetUserGameByUserIdAsync(int userId);
}
