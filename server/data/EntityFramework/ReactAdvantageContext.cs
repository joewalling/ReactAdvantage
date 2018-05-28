using Microsoft.EntityFrameworkCore;
using ReactAdvantage.Domain.Entities;

namespace ReactAdvantage.Data.EntityFramework
{
    public class ReactAdvantageContext : DbContext
    {
        public ReactAdvantageContext(DbContextOptions<ReactAdvantageContext> options) : base(options)
        {

        }
        public ReactAdvantageContext(string dbName)
        {
            _dbName = dbName;
        }

        private readonly string _dbName;

        public virtual DbSet<Project> Projects { get; set; }
        public virtual DbSet<Task> Tasks { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);

            if (!string.IsNullOrEmpty(_dbName))
                optionsBuilder.UseSqlServer(_dbName);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //    new AlbumConfiguration(modelBuilder.Entity<Album>());
            //    new ArtistConfiguration(modelBuilder.Entity<Artist>());
            //}
        }
    }
}
