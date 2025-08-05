using Business.Contract.IServices;
using Business.Models;
using Business.Models.Request;
using DataAccess.Repositories.IRepo;
using Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Business.Contract.Services
{
    public class CarService : ICarService
    {
        private readonly ICarRepo _repository;

        public CarService(ICarRepo repository)
        {
            _repository = repository;
        }

        public async Task<List<Car>> GetAllAsync()
            => await _repository.GetAllAsync();

        public async Task<Car> GetByIdAsync(Guid id)
            => await _repository.GetByIdAsync(id);

        public async Task<PaginatedList<Car>> CarPagination(PagedRequest<object?> pagedRequest)
        {
            var query = await _repository.GetCarAllQueryable();

            // Search
            if (!string.IsNullOrEmpty(pagedRequest.SearchQuery))
            {
                var searchQuery = pagedRequest.SearchQuery.ToLower();
                bool isModelNo = int.TryParse(searchQuery, out int modelNo);

                query = query.Where(x =>
                    x.Classes.ToLower().Contains(searchQuery) ||
                    x.Brand.ToLower().Contains(searchQuery) ||
                    x.Model.ToLower().Contains(searchQuery) ||
                    (isModelNo && x.Model_No == modelNo)
                );
            }

            // Total count before pagination
            int totalRecords = await query.CountAsync();

            // Sorting
            if (!string.IsNullOrEmpty(pagedRequest.SortColumn))
            {
                query = pagedRequest.IsAscending
                    ? query.OrderBy(e => EF.Property<object>(e, pagedRequest.SortColumn))
                    : query.OrderByDescending(e => EF.Property<object>(e, pagedRequest.SortColumn));
            }
            else
            {
                query = query.OrderBy(x => x.CarId); // Default sort
            }

            // Pagination
            var items = await query
                .Skip((pagedRequest.PageIndex - 1) * pagedRequest.PageSize)
                .Take(pagedRequest.PageSize)
                .ToListAsync();

            return new PaginatedList<Car>(items, totalRecords, pagedRequest.PageIndex, pagedRequest.PageSize);
        }

        public async Task AddAsync(AddCarsRequest car)
        {
            var data = new Car
            {
                Classes = car.Classes,
                Brand = car.Brand,
                Activity = car.Activity,
                Date = car.Date,
                Features = car.Features,
                Model = car.Model,
                Model_No = car.Model_No,
                Price = car.Price,
            };

            await _repository.AddAsync(data);
        }

        public async Task UpdateAsync(UpdateCarsRequest car)
        {
            var data = await _repository.GetByIdAsync(car.CarId);

            if (data == null)
                return; // Optionally handle not found case

            data.Classes = car.Classes;
            data.Brand = car.Brand;
            data.Activity = car.Activity;
            data.Date = car.Date;
            data.Features = car.Features;
            data.Model = car.Model;
            data.Model_No = car.Model_No;
            data.Price = car.Price;

            await _repository.UpdateAsync(data);
        }

        public async Task DeleteAsync(Guid id)
            => await _repository.DeleteAsync(id);
    }
}
