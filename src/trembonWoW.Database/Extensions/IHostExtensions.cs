using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace trembonWoW.Database.Extensions;
public static class IHostExtensions
{
    public static void ApplyDatabaseMigrations(this IHost host)
    {
        host.ApplyDatabaseMigration<DefaultContext>();
    }

    private static void ApplyDatabaseMigration<TContext>(this IHost host) where TContext : DbContext
    {
        using var scope = host.Services.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<TContext>();
        context.Database.Migrate();
    }
}
