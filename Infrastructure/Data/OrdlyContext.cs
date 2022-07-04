using Microsoft.EntityFrameworkCore;
using WebApi.Models;
namespace Infrastructure.Data;

public class OrdlyContext : DbContext
{
    public OrdlyContext(DbContextOptions<OrdlyContext> options) : base(options)
    { }
    public DbSet<User> Users { get; set; }
    public DbSet<Word> Words { get; set; }
}