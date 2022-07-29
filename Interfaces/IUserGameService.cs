using OrdlyBackend.DTOs;
using OrdlyBackend.DTOs.v2;
using OrdlyBackend.Models;

namespace OrdlyBackend.Interfaces;
public interface IUserGameService
{
    Task<bool> AddGuessAsync(GuessRequest request, DailyWord daily, List<Word> allWords, GuessResponse2 guessResonse);
    Task<UserGame> GetUserGameByUserIdAsync(int userId);
}
