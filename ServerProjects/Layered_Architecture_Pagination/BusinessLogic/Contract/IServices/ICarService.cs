using Business.Models;
using Business.Models.Request;
using Business.Models.Response;
using Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Contract.IServices
{
    public interface ICarService
    {
        Task<List<Car>> GetAllAsync();
        Task<Car> GetByIdAsync(Guid id);
        Task AddAsync(AddCarsRequest car);
        Task UpdateAsync(UpdateCarsRequest car);
        Task DeleteAsync(Guid id);
        Task<PaginatedList<Car>> CarPagination(PagedRequest<object?> pagedRequest);

    }
}
