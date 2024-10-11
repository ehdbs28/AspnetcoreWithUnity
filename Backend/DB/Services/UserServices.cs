using Backend.DB.Models;
using Microsoft.EntityFrameworkCore;

namespace Backend.DB.Services;

public class UserServices
{
    private readonly GameDBContext _context;

    public UserServices(GameDBContext context)
    {
        _context = context;
    }

    public async Task<User> AddUser(string userName, string password)
    {
        var newUser = new User
        {
            UserName = userName,
            Password = password
        };

        await _context.Users.AddAsync(newUser);
        await _context.SaveChangesAsync();
        return newUser;
    }

    public async Task<List<User>> GetAllUsersAsync()
    {
        return await _context.Users.ToListAsync();
    }

    public async Task<User?> GetUserByUserNameAsync(string userName)
    {
        return await (from user in _context.Users
            where user.UserName == userName select user).FirstOrDefaultAsync();
    }

    public async Task<User?> GetUserByIdAsync(int userId)
    {
        return await (from user in _context.Users where user.Id == userId select user).FirstOrDefaultAsync();
    }
}