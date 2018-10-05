using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using ReactAdvantage.Domain.Models;

namespace ReactAdvantage.Data
{
    public class ReactAdvantageContext : IdentityDbContext<User>
    {
        public ILogger Logger { get; }
        
        public ReactAdvantageContext(DbContextOptions<ReactAdvantageContext> options, ILogger<ReactAdvantageContext> logger)
            : base(options)
        {
            Logger = logger;
        }

        public DbSet<Tenant> Tenants { get; set; }

        public DbSet<Project> Projects { get; set; }

        public DbSet<Task> Tasks { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<Task>()
                .HasOne(x => x.Tenant)
                .WithMany()
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<User>()
                .HasOne(x => x.Tenant)
                .WithMany()
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<Project>()
                .HasOne(x => x.Tenant)
                .WithMany()
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}