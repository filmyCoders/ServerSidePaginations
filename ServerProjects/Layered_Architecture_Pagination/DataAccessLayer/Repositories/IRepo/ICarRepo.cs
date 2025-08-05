using Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.ConstrainedExecution;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Repositories.IRepo
{
    public interface ICarRepo
    {
        Task<List<Car>> GetAllAsync();
        Task<Car> GetByIdAsync(Guid id);
        Task AddAsync(Car car);
        Task UpdateAsync(Car car);
        Task DeleteAsync(Guid id);

        Task<IQueryable<Car>> GetCarAllQueryable();

        Task<IQueryable<Car>> GetCarAllQueryable(Expression<Func<Car, bool>> predicate);

    }
}
