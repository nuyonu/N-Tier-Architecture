using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using N_Tier.Infrastructure.Persistence;
using System;

namespace N_Tier.Common
{
    public static class AutomatedMigration
    {
        public static void Migrate(IServiceProvider services)
        {
            var context = services.GetRequiredService<DatabaseContext>();

            if (context.Database.IsSqlServer())
            {
                context.Database.Migrate();
            }
        }
    }
}
