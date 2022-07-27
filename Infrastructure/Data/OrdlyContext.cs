using Microsoft.EntityFrameworkCore;
using OrdlyBackend.Models;

namespace OrdlyBackend.Infrastructure.Data;

public class OrdlyContext : DbContext
{
    public DbSet<User> Users { get; set; }
    public DbSet<Word> Words { get; set; }
    public DbSet<DailyWord> DailyWords { get; set; }
    public DbSet<UserGame> UserGames { get; set; }
    public DbSet<Guess> Guesses { get; set; }
    public DbSet<Rank> Ranks { get; set; }
    public DbSet<UserRank> UserRanks { get; set; }
    public OrdlyContext(DbContextOptions<OrdlyContext> options) : base(options)
    {
     
    }

    //protected override void OnModelCreating(ModelBuilder modelBuilder)
    //{
    //    modelBuilder.Entity<Guess>()
    //        .HasKey(c => new { c. });
    //}
}