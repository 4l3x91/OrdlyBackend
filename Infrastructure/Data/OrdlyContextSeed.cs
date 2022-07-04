using Microsoft.EntityFrameworkCore;
using WebApi.Models;
using WebApi;

namespace Infrastructure.Data
{
    public static class OrdlyContextSeed
    {
        public static async void Initialize(IServiceProvider serviceProvider)
        {
            using (var context = new OrdlyContext(
                serviceProvider.GetRequiredService<
                    DbContextOptions<OrdlyContext>>()))
            {
                if (!context.Users.Any()) await GenerateUsers(context);   // DB has been seeded
            }
        }
        private static async Task GenerateUsers(OrdlyContext context)
        {
            context.Users.AddRange(
                                new User
                                {
                                    Id = 1,
                                    TotalGames = "4",
                                    TotalWins = "2",
                                    CurrentStreak = "2",
                                    LongestStreak = "2"
                                },
                                new User
                                {
                                    Id = 2,
                                    TotalGames = "5",
                                    TotalWins = "4",
                                    CurrentStreak = "3",
                                    LongestStreak = "3"
                                }
                            );

            context.SaveChangesAsync();
        }
    }
}