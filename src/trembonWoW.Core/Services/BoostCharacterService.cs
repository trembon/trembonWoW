using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using trembonWoW.Core.Connectors.Characters;
using trembonWoW.Core.Connectors.RemoteAccess;
using trembonWoW.Core.Extensions;
using trembonWoW.Core.Models;
using trembonWoW.Database;

namespace trembonWoW.Core.Services;

public interface IBoostCharacterService
{
    Task<List<EligableBoost>> GetEligableBoosts(int accountId);

    Task<List<EligableCharacter>> GetEligableCharaceters(int accountId);
}

public class BoostCharacterService(DefaultContext defaultContext, IRemoteAccessSoapAPI remoteAccessSoapAPI, ICharacterService characterService) : IBoostCharacterService
{
    public async Task<List<EligableBoost>> GetEligableBoosts(int accountId)
    {
        var availableBoosts = await defaultContext.BoostCharacterTemplate.OrderBy(x => x.SetToLevel).ToListAsync();
        var usedBoosts = await defaultContext.BoostedCharacters.Where(x => x.AccountID == accountId).ToListAsync();

        DateTime weekAgo = DateTime.UtcNow.AddDays(-7);
        DateTime monthAgo = DateTime.UtcNow.AddMonths(-1);

        List<EligableBoost> eligableBoosts = [];
        foreach (var boost in availableBoosts)
        {
            EligableBoost eligableBoost = new()
            {
                TemplateID = boost.ID,
                Name = boost.Name,
                Level = boost.SetToLevel,
                IsEligable = true
            };

            if (!usedBoosts.Any(x => x.BoostedAt > weekAgo))
            {
                eligableBoost.IsEligable = false;
                eligableBoost.NotEligableReason = "Can only boost a character once a week";
            }
            else if (!usedBoosts.Any(x => x.TemplateID == boost.ID && x.BoostedAt > monthAgo))
            {
                eligableBoost.IsEligable = false;
                eligableBoost.NotEligableReason = "A specific boost can only be used once a month";
            }

            eligableBoosts.Add(eligableBoost);
        }

        return eligableBoosts;
    }

    public async Task<List<EligableCharacter>> GetEligableCharaceters(int accountId)
    {
        var characters = await characterService.GetAllForAccount(accountId);

        List<EligableCharacter> eligableCharaceters = [];
        foreach (var character in characters)
        {
            EligableCharacter eligableCharacter = new() {
                CharacterID = character.ID,
                Name = character.Name,
                Race = character.Race,
                Level = character.Level,
                IsEligable = true
            };

            if(character.Level > 1)
            {
                eligableCharacter.IsEligable = false;
                eligableCharacter.NotEligableReason = "Only new (level 1) characters can be boosted";
            }

            eligableCharaceters.Add(eligableCharacter);
        }

        return eligableCharaceters;
    }

    private const string BOOST_MONEY_SUBJECT = "Boost money";
    private const string BOOST_MONEY_TEXT = "Money to get you started. Should cover class spell, mount and some gear on the auction house.";

    public async Task<bool> BoostCharacter(int accountId, int characterId, Guid boostTemplateId)
    {
        var eligableBoosts = await GetEligableBoosts(accountId);

        var eligableBoost = eligableBoosts.FirstOrDefault(x => x.TemplateID == boostTemplateId);
        if (eligableBoost is null || !eligableBoost.IsEligable)
            throw new InvalidOperationException("Account and/or character is not eligable for this boost");

        var eligableCharacters = await GetEligableCharaceters(accountId);

        var eligableCharacter = eligableCharacters.FirstOrDefault(x => x.CharacterID == characterId);
        if(eligableCharacter is null || !eligableCharacter.IsEligable)
            throw new InvalidOperationException("Character is not eligable for boosting");

        var boostTemplate = await defaultContext.BoostCharacterTemplate.FirstAsync(x => x.ID == boostTemplateId);

        bool setLevel = await remoteAccessSoapAPI.CharacterLevel(eligableCharacter.Name, boostTemplate.SetToLevel);
        bool sendMoney = await remoteAccessSoapAPI.SendMoney(eligableCharacter.Name, boostTemplate.GoldToSend, BOOST_MONEY_SUBJECT, BOOST_MONEY_TEXT);

        bool teleport = false;
        if (eligableCharacter.Race.IsAlliance())
        {
            teleport = await remoteAccessSoapAPI.TeleportCharacter(eligableCharacter.Name, boostTemplate.TeleportToAlliance);
        }
        else
        {
            teleport = await remoteAccessSoapAPI.TeleportCharacter(eligableCharacter.Name, boostTemplate.TeleportToHorde);
        }

        return setLevel && sendMoney && teleport;
    }
}
