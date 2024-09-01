using Microsoft.EntityFrameworkCore;

namespace RedisExampleApp.API.Model
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Product>().HasData(
                new Product() { Id = 1, Name = "Tablet", Price = 7850 },
                new Product() { Id = 2, Name = "Telefon", Price = 12500 },
                new Product() { Id = 3, Name = "Televizyon", Price = 8640 },
                new Product() { Id = 4, Name = "Monitör", Price = 3780 }
                );
        }

        public DbSet<Product> Products { get; set; }
    }
}
