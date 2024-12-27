using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace trembonWoW.Database.Entities;

public class BoostedCharacters
{
    public Guid ID { get; set; }

    public int CharacterID { get; set; }

    public Guid TemplateID { get; set; }

    public required BoostCharacterTemplates Template { get; set; }

    public DateTime BoostedAt { get; set; }
}
