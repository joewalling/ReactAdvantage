using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using ReactAdvantage.Domain.Models;

namespace ReactAdvantage.Data
{
    public class ReactAdvantageContext : IdentityDbContext<User>
    {
        public ILogger Logger { get; }
        
        public ReactAdvantageContext(DbContextOptions options, ILogger<ReactAdvantageContext> logger)
            : base(options)
        {
            Logger = logger;
        }

        public DbSet<Project> Projects { get; set; }

        public DbSet<Task> Tasks { get; set; }

    }
}