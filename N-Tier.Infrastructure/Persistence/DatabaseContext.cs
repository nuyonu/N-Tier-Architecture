using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using N_Tier.Core.Common;
using N_Tier.Core.Entities;
using N_Tier.Infrastructure.Identity;
using N_Tier.Shared.Services;
using System;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;

namespace N_Tier.Infrastructure.Persistence
{
    public class DatabaseContext : IdentityDbContext<ApplicationUser>
    {
        private readonly IClaimService _claimService;

        public DatabaseContext(DbContextOptions options, IClaimService claimService) : base(options)
        {
            _claimService = claimService;
        }

        public DbSet<TodoItem> TodoItems { get; set; }

        public DbSet<TodoList> TodoLists { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

            base.OnModelCreating(builder);
        }

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken())
        {
            foreach (var entry in ChangeTracker.Entries<AdvancedBaseEntity>())
            {
                switch (entry.State)
                {
                    case EntityState.Added:
                        entry.Entity.CreatedBy = _claimService.GetUserId();
                        entry.Entity.CreatedOn = DateTime.Now;
                        break;

                    case EntityState.Modified:
                        entry.Entity.UpdatedBy = _claimService.GetUserId();
                        entry.Entity.UpdatedOn = DateTime.Now;
                        break;
                }
            }

            return await base.SaveChangesAsync(cancellationToken);
        }
    }
}
