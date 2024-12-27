using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace trembonWoW.Database.Extensions;

public static class IServiceCollectionExtensions
{
    public static void AddDefaultDatabaseContext(this IServiceCollection services, string postgressConnectionString)
    {
        string assemblyName = Assembly.GetExecutingAssembly().GetName().Name!;
        services.AddDbContext<DefaultContext>(options => options.UseNpgsql(postgressConnectionString, x => x.MigrationsAssembly(assemblyName)), ServiceLifetime.Transient);
    }
}
