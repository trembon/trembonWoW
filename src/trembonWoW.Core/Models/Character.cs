using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using trembonWoW.Core.Models.Enums;

namespace trembonWoW.Core.Models;

public class Character(Connectors.Characters.Records.Character character)
{
    public int ID { get; set; } = (int)character.Guid;

    public int AccountID { get; set; } = (int)character.Account;

    public string Name { get; set; } = character.Name;

    public int Level { get; set; } = character.Level;

    public Race Race { get; set; } = (Race)character.Race;

    public Class Class { get; set; } = (Class)character.Class;
}