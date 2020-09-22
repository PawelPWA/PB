using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace SimpleCarCatalogue.Repositories
{
    public interface ICarContext
    {
        DbSet<Car> Cars { get; set; }
        DbSet<Producer> Producers { get; set; }

        void SaveChanges();
        Task SaveChangesAsync();
    }
}