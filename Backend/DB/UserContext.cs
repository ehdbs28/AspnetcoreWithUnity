using Backend.DB.Models;
using Microsoft.EntityFrameworkCore;

namespace Backend.DB;

public class UserContext : DbContext
{
    public UserContext(DbContextOptions<UserContext> options) : base(options)
    {
    }
    
    public DbSet<User> Users { get; set; }
}