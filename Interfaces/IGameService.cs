using OrdlyBackend.DTOs.v1;

namespace OrdlyBackend.Interfaces
{
    public interface IGameService
    {
        public Task<GuessResponse> GetFullGuessResultAsync(GuessRequest request);
    }
}
