using Microsoft.EntityFrameworkCore;
using OrdlyBackend.Infrastructure.Data;
using OrdlyBackend.Interfaces;
using OrdlyBackend.Models;
using OrdlyBackend.DTOs;
using OrdlyBackend.DTOs.v1;

namespace OrdlyBackend.Services;

public class RankService : IRankService
{
    OrdlyContext _context;
    IUserService _userService;

    public RankService(OrdlyContext context, IUserService userService)
    {
        _context = context;
        _userService = userService;
    }

    public async Task<UserRank> AddOrUpdateUserRankAsync(UserRank userRequest)
    {
        _context.Update(userRequest);
        await _context.SaveChangesAsync();
        return userRequest;
    }

    public async Task<UserRankDTO> AddRatingAsync(BaseUserDTO userRequest)
    {
        var userRank = await GetUserRankAsync(userRequest);
        if (userRank == null)
        {
            await AddOrUpdateUserRankAsync(userRank);
        };

        // TODO: Calculate correct rating
        userRank.Rating += 10;
        _context.Update(userRank);
        await _context.SaveChangesAsync();

        return await GetRankAsync(userRequest, userRank);
    }

    public async Task<UserRankDTO> SubtractRatingAsync(BaseUserDTO userRequest)
    {
        var userRank = await GetUserRankAsync(userRequest);
        if (userRank == null) return null;
        
        // TODO: Calculate correct rating
        userRank.Rating -= 10;
        _context.Update(userRank);
        await _context.SaveChangesAsync();

        return await GetRankAsync(userRequest, userRank);
    }



    private async Task<UserRank> GetUserRankAsync(BaseUserDTO userRequest)
    {
        // TODO: Add try-catch
        var user = await _userService.GetUserByIdAsync(userRequest.UserId);
        if (user.UserKey == userRequest.UserKey)
        {
            return await _context.UserRanks.FirstOrDefaultAsync(x => x.UserId == user.Id);
        }
        return null;
    }

    private async Task<UserRankDTO> GetRankAsync(BaseUserDTO userRequest, UserRank userRank)
    {
        var currentRank = await _context.Ranks.FirstOrDefaultAsync(x => x.MaxRating <= userRank.Rating);

        return new UserRankDTO()
        {
            Id = userRequest.UserId,
            Rating = userRank.Rating,
            Rank = currentRank.RankName
        };
    }
}