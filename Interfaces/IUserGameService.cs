using OrdlyBackend.DTOs.v1;
using OrdlyBackend.DTOs.v1.UserGameDTOs;
using OrdlyBackend.Models;

namespace OrdlyBackend.Interfaces;
public interface IUserGameService
{
    Task<bool> CreateGuessAsync(GuessRequest request, DailyWord daily, List<Word> allWords, GuessResponse guessResonse);
    Task<UserGame> FetchUserGameAsync(int userId, int dailyId);
    Task<List<UserGame>> GetAllUserGamesAsync();
}
