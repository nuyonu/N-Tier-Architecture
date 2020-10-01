using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore.Internal;
using N_Tier.DataAccess.Identity;
using System.Threading.Tasks;

namespace N_Tier.DataAccess.Persistence
{
    public static class DatabaseContextSeed
    {
        public static async Task SeedDatabaseAsync(DatabaseContext context, UserManager<ApplicationUser> userManager)
        {
            if (!userManager.Users.Any())
            {
                var user = new ApplicationUser { UserName = "admin", Email = "admin@admin.com" };

                await userManager.CreateAsync(user, "Admin123.?");
            }

            await context.SaveChangesAsync();
        }
    }
}
