using CRUDOperations.Models;
using Microsoft.EntityFrameworkCore;

namespace CRUDOperations.Db_Context
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<Car> Cars { get; set; }
    }
}
