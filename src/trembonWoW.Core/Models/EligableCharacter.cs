using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using trembonWoW.Core.Models.Enums;

namespace trembonWoW.Core.Models;

public class EligableCharacter
{
    public int CharacterID { get; set; }

    public required string Name { get; set; }

    public Race Race { get; set; }

    public int Level { get; set; }

    public bool IsEligable { get; set; }

    public string? NotEligableReason { get; set; }
}
