using Backend.DB.Models;
using Microsoft.EntityFrameworkCore;

namespace Backend.DB.Services;

public class CharacterServices
{
    private readonly GameDBContext _context;

    public CharacterServices(GameDBContext context)
    {
        _context = context;
    }

    public async Task AddCharacter(Character newCharacter)
    {
        await _context.Characters.AddAsync(newCharacter);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateCharacter(Character character)
    {
        var dbCharacter = await GetCharacterByUserIdAsync(character.OwnerUserId);
        dbCharacter.Level = character.Level;
        dbCharacter.LastPosition = character.LastPosition;
        await _context.SaveChangesAsync();
    }

    public async Task<Character?> GetCharacterByUserIdAsync(int userId)
    {
        return await (from character in _context.Characters where character.OwnerUserId == userId select character)
            .FirstOrDefaultAsync();
    }
}