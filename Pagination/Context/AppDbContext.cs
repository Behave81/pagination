using Microsoft.EntityFrameworkCore;
using Pagination.Models;

namespace Pagination.Context
{
    public class AppDbContext : DbContext
    {
        public DbSet<Entity> entities { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseNpgsql("Host=raspberrypi;Database=pagination;Username=postgres;Password=manager");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Entity>().ToTable("EntityTable");


            modelBuilder.Entity<Entity>().HasKey(e => e.Id);

        }
    }
}
