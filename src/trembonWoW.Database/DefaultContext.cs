using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using trembonWoW.Database.Entities;

namespace trembonWoW.Database;

public class DefaultContext(DbContextOptions<DefaultContext> options) : DbContext(options)
{
    public DbSet<BoostedCharacters> BoostedCharacters { get; set; }

    public DbSet<BoostCharacterTemplates> BoostCharacterTemplate { get; set; }
}
