using OrdlyBackend.DTOs;
using OrdlyBackend.DTOs.v2;

namespace OrdlyBackend.Interfaces
{
    public interface IRankService
    {
        Task<UserRankDTO> AddRatingAsync(BaseUserDTO baseUserDTO);
        Task<UserRankDTO> SubtractRatingAsync(BaseUserDTO baseUserDTO);
    }
}
