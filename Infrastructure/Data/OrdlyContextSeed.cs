using Microsoft.EntityFrameworkCore;
using OrdlyBackend.Models;

namespace OrdlyBackend.Infrastructure.Data
{
    public static class OrdlyContextSeed
    {
        public static async void Initialize(IServiceProvider serviceProvider)
        {
            using (var context = new OrdlyContext(
                serviceProvider.GetRequiredService<
                    DbContextOptions<OrdlyContext>>()))
            {
                if (context.Words.Count() == 0) await GenerateWords(context);   // DB has been seeded
            }
        }
        private static async Task GenerateWords(OrdlyContext context)
        {
            List<Word> words = new List<Word>();
            string[] lines = File.ReadAllLines("words.txt");

            for (int i = 0; i < lines.Length; i++)
            {
                Word newWord = new Word
                {
                    Id = 0,
                    Name = lines[i],
                    Category = "basic",
                    RowCreated = DateTime.Now,
                    RowModified = DateTime.Now,
                    RowVersion = 1
                };
                words.Add(newWord);
                
            }
            await context.AddRangeAsync(words);
            await context.SaveChangesAsync();
        }
    
    }
}