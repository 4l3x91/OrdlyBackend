using OrdlyBackend.DTOs;
using OrdlyBackend.DTOs.v1;

namespace OrdlyBackend.Interfaces
{
    public interface IRankService
    {
        Task<UserRankDTO> AddRatingAsync(BaseUserDTO baseUserDTO);
        Task<UserRankDTO> SubtractRatingAsync(BaseUserDTO baseUserDTO);
    }
}
