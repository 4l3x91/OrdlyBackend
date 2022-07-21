using OrdlyBackend.DTOs;

namespace OrdlyBackend.Interfaces
{
    public interface IGameService
    {
        public Task<GuessResponse> GetGuessResultAsync(GuessRequest request);
    }
}
