using System;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.Extensions.Logging;
using ReactAdvantage.Domain;
using ReactAdvantage.Domain.Models;
using ReactAdvantage.Domain.MultiTenancy;
using ReactAdvantage.Domain.Services;

namespace ReactAdvantage.Data
{
    public class ReactAdvantageContext : IdentityDbContext<User>
    {
        public ILogger Logger { get; }

        public bool IsTenantFilterEnabled { get; private set; }

        public int? TenantFilterValue { get; private set; }

        public ReactAdvantageContext(
            DbContextOptions<ReactAdvantageContext> options,
            ILogger<ReactAdvantageContext> logger,
            ITenantProvider tenantProvider
            )
            : base(options)
        {
            Logger = logger;
            TenantFilterValue = tenantProvider.GetTenantId();
            IsTenantFilterEnabled = true;
        }

        public IDisposable DisableTenantFilter()
        {
            var oldValue = IsTenantFilterEnabled;
            IsTenantFilterEnabled = false;
            return new DisposeAction(() => IsTenantFilterEnabled = oldValue);
        }

        public IDisposable SetTenantFilterValue(int? newValue)
        {
            var oldValue = TenantFilterValue;
            TenantFilterValue = newValue;
            return new DisposeAction(() => TenantFilterValue = oldValue);
        }

        public DbSet<Tenant> Tenants { get; set; }

        public DbSet<Project> Projects { get; set; }

        public DbSet<Task> Tasks { get; set; }
        
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<User>(u =>
            {
                u.Metadata.RemoveIndex(u.HasIndex(x => x.NormalizedUserName).Metadata.Properties);
                u.HasIndex(x => new { x.NormalizedUserName, x.TenantId })
                    .HasName("UserNameIndex")
                    .IsUnique();

                u.Metadata.RemoveIndex(u.HasIndex(x => x.NormalizedEmail).Metadata.Properties);
                u.HasIndex(x => new { x.NormalizedEmail, x.TenantId })
                    .HasName("EmailIndex")
                    .IsUnique();
            });

            builder.Entity<User>()
                .HasQueryFilter(x => !IsTenantFilterEnabled || x.TenantId == TenantFilterValue)
                .HasOne(x => x.Tenant)
                .WithMany()
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<Task>()
                .HasQueryFilter(x => !IsTenantFilterEnabled || x.TenantId == TenantFilterValue)
                .HasOne(x => x.Tenant)
                .WithMany()
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<Project>()
                .HasQueryFilter(x => !IsTenantFilterEnabled || x.TenantId == TenantFilterValue)
                .HasOne(x => x.Tenant)
                .WithMany()
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}