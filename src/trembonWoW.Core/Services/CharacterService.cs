using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using trembonWoW.Core.Connectors.Characters;
using trembonWoW.Core.Models;

namespace trembonWoW.Core.Services;

public interface ICharacterService
{
    Task<IEnumerable<Character>> GetAllForAccount(int accountId);
}

public class CharacterService(ICharactersDatabase charactersDatabase) : ICharacterService
{
    public async Task<IEnumerable<Character>> GetAllForAccount(int accountId)
    {
        ArgumentOutOfRangeException.ThrowIfLessThan(accountId, 1, nameof(accountId));

        var result = await charactersDatabase.GetCharacters((uint)accountId);
        var converted = result.Select(x => new Character(x)).ToList();
        return converted;
    }
}