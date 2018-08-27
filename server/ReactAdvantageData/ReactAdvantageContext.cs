using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using ReactAdvantage.Domain.Models;

namespace ReactAdvantage.Data
{
    public class ReactAdvantageContext : DbContext
    {
        public ILogger Logger { get; }
        
        public ReactAdvantageContext(DbContextOptions options, ILogger<ReactAdvantageContext> logger)
            : base(options)
        {
            Logger = logger;
            Database.EnsureCreated();
        }

        public DbSet<User> Users { get; set; }

        public DbSet<Project> Projects { get; set; }

        public DbSet<Task> Tasks { get; set; }

    }
}