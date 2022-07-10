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
                if(!context.Words.Any()) await GenerateWords(context);   // DB has been seeded
            }
        }
        private static async Task GenerateWords(OrdlyContext context)
        {
            
            string[] lines = File.ReadAllLines("words.txt");
            int idCounter = 1;

            for (int i = 0; i < lines.Length; i++)
            {
                Word newWord = new Word {
                    WordId = idCounter,
                    Name = lines[i],
                    Date = DateTime.Today.AddDays(idCounter).ToShortDateString()
                };
                idCounter++;
                context.Add(newWord);
            }
                await context.SaveChangesAsync();
        }
        private static async Task GenerateUsers(OrdlyContext context)
        {
            context.Users.AddRange(
                                new User
                                {
                                    UserId = 1,
                                    TotalGames = "4",
                                    TotalWins = "2",
                                    CurrentStreak = "2",
                                    LongestStreak = "2"
                                },
                                new User
                                {
                                    UserId = 2,
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