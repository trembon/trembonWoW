using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace trembonWoW.Database.Entities;

public class ListedFile
{
    public Guid ID { get; set; }

    public required string Description { get; set; }

    public required string Filename { get; set; }

    public required string Url { get; set; }

    public required string Category { get; set; }

    public int SortOrder { get; set; }
}
