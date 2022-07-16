using Microsoft.EntityFrameworkCore;
using OrdlyBackend.Models;

namespace OrdlyBackend.Infrastructure.Data;

public class OrdlyContext : DbContext
{
    public OrdlyContext(DbContextOptions<OrdlyContext> options) : base(options)
    {

    }
    public DbSet<User> Users { get; set; }
    public DbSet<Word> Words { get; set; }
    public DbSet<DailyWord> DailyWords { get; set; }
}