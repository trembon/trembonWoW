using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace trembonWoW.Core.Models;

public class EligableBoost
{
    public Guid TemplateID { get; set; }

    public required string Name { get; set; }

    public int Level { get; set; }

    public bool IsEligable { get; set; }

    public string? NotEligableReason { get; set; }
}
