using Microsoft.EntityFrameworkCore;
using SimpleCarCatalogue.Repositories;
using System;
using System.Threading.Tasks;

namespace SimpleCarCatalogue.Repositories
{
    public class CarContext : DbContext, ICarContext
    {
        public CarContext(DbContextOptions<CarContext> options)
            : base(options)
        {

        }

        public DbSet<Car> Cars { get; set; }
        public DbSet<Producer> Producers { get; set; }

        public Task SaveChangesAsync()
        {
            return SaveChangesAsync();
        }

        void ICarContext.SaveChanges()
        {
            SaveChanges();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Producer>().HasData(
                new Producer
                {
                    Id = Guid.NewGuid(),
                    Name = "Alfa Romeo",
                },
                new Producer
                {
                    Id = Guid.NewGuid(),
                    Name = "Audi",
                },
                new Producer
                {
                    Id = Guid.NewGuid(),
                    Name = "Volkswagen",
                });
        }
    }
}
