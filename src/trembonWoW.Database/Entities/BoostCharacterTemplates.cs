using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace trembonWoW.Database.Entities;

public class BoostCharacterTemplates
{
    public Guid ID { get; set; }

    public required string Name { get; set; }

    public int SetToLevel { get; set; }

    public int GoldToSend { get; set; }

    public required string TeleportToHorde { get; set; }

    public required string TeleportToAlliance { get; set; }
}
