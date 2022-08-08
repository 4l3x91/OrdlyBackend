using OrdlyBackend.DTOs.v1.UserGameDTOs;
using OrdlyBackend.Models;

namespace OrdlyBackend.Interfaces
{
    public interface IObjectBuilder
    {
        int[] GenerateResult(string guess, string dailyWord);
        Task<UserHistoryDTO> GetUserHistoryAsync(List<UserGame> usersAllGames);
    }
}
