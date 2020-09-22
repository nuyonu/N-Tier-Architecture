using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using N_Tier.Core.Entities;
using N_Tier.Infrastructure.Identity;
using System.Reflection;

namespace N_Tier.Infrastructure.Persistence
{
    public class DatabaseContext : IdentityDbContext<ApplicationUser>
    {
        public DatabaseContext(DbContextOptions options) : base(options)
        { }

        public DbSet<TodoItem> TodoItems { get; set; }

        public DbSet<TodoList> TodoLists { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

            base.OnModelCreating(builder);
        }
    }
}
