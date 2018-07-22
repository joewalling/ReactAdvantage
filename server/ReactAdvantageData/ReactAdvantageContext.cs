using Microsoft.EntityFrameworkCore;
using ReactAdvantage.Domain.Models;

namespace ReactAdvantage.Data
{
    public class ReactAdvantageContext : DbContext
    {
        public ReactAdvantageContext(DbContextOptions options)
            : base(options)
        {
            Database.EnsureCreated();
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Project> Projects { get; set; }

        public DbSet<Task> Tasks { get; set; }

    }
}