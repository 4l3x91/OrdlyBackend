using OrdlyBackend.DTOs;
using OrdlyBackend.DTOs.v1;
using OrdlyBackend.Models;

namespace OrdlyBackend.Interfaces
{
    public interface IRankService
    {
        Task<UserRankDTO> AddRatingAsync(BaseUserDTO baseUserDTO);
        Task<UserRankDTO> SubtractRatingAsync(BaseUserDTO baseUserDTO);
        Task<UserRank> AddOrUpdateUserRankAsync(UserRank userRank);
    }
}