using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using ReactAdvantage.Domain.Models;

namespace ReactAdvantage.Data
{
    public class ReactAdvantageContext : IdentityDbContext<User>
    {
        public ReactAdvantageContext(DbContextOptions options)
            : base(options)
        {
        }

        public DbSet<Project> Projects { get; set; }

        public DbSet<Task> Tasks { get; set; }

    }
}