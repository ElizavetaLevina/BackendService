using BackendService.DAL.Configurations;
using Microsoft.EntityFrameworkCore;

namespace BackendService.DAL.Models
{
    public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : DbContext(options)
    {
        public DbSet<PostEntity> Posts { get; set; }
        public DbSet<TagEntity> Tags { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfiguration(new PostConfiguration());
            modelBuilder.ApplyConfiguration(new TagConfiguration());
        }
    }
}
