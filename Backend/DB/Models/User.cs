using Microsoft.EntityFrameworkCore;

namespace Backend.DB.Models;

public class User
{
    public int Id { get; set; }
    public string UserName { get; set; }
    public string Password { get; set; }
}