using Backend.DB.Models;
using Microsoft.EntityFrameworkCore;

namespace Backend.DB;

public class GameDBContext : DbContext
{
    public GameDBContext(DbContextOptions<GameDBContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.Entity<Character>().HasKey(c => c.OwnerUserId);
    }
    
    public DbSet<User> Users { get; set; }
    public DbSet<Character> Characters { get; set; }
}