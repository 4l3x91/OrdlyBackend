using OrdlyBackend.DTOs;

namespace OrdlyBackend.Interfaces
{
    public interface IGameService
    {
        public Task<GuessResponse> GetGuessResultAsync(GuessRequest request);
        public Task<DTOs.v2.GuessResponse2> GetFullGuessResultAsync(GuessRequest request);
    }
}
