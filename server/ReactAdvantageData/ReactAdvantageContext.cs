using System;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using ReactAdvantage.Api.Services;
using ReactAdvantage.Domain;
using ReactAdvantage.Domain.Models;

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

            builder.Entity<Task>()
                .HasQueryFilter(x => !IsTenantFilterEnabled || x.TenantId == TenantFilterValue)
                .HasOne(x => x.Tenant)
                .WithMany()
                //.HasForeignKey(x => x.TenantId)
                //.IsRequired()
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<User>()
                //.HasQueryFilter(x => x.TenantId == _tenantId || _tenantId == null) //show all users to the host admin
                .HasQueryFilter(x => !IsTenantFilterEnabled || x.TenantId == TenantFilterValue)
                .HasOne(x => x.Tenant)
                .WithMany()
                //.HasForeignKey(x => x.TenantId)
                //.IsRequired(false)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<Project>()
                .HasQueryFilter(x => !IsTenantFilterEnabled || x.TenantId == TenantFilterValue)
                .HasOne(x => x.Tenant)
                .WithMany()
                //.HasForeignKey(x => x.TenantId)
                //.IsRequired()
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}