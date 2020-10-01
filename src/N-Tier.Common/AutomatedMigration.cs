using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using N_Tier.Infrastructure.Identity;
using N_Tier.Infrastructure.Persistence;
using System;
using System.Threading.Tasks;

namespace N_Tier.Common
{
    public static class AutomatedMigration
    {
        public static async Task MigrateAsync(IServiceProvider services)
        {
            var context = services.GetRequiredService<DatabaseContext>();

            if (context.Database.IsSqlServer())
            {
                context.Database.Migrate();
            }

            var userManager = services.GetRequiredService<UserManager<ApplicationUser>>();

            await DatabaseContextSeed.SeedDatabaseAsync(context, userManager);
        }
    }
}
