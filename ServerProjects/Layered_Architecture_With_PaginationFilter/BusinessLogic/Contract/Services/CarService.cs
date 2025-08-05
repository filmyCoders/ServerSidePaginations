using Business.Contract.IServices;
using Business.Models;
using Business.Models.Request;
using Business.Models.Response;
using DataAccess.Repositories.IRepo;
using DataAccess.Repositories.Repo;
using Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Runtime.ConstrainedExecution;
using System.Text;
using System.Threading.Tasks;

namespace Business.Contract.Services
{
    public class CarService:ICarService
    {
        private readonly ICarRepo _repository;

        public CarService(ICarRepo repository)
        {
            _repository = repository;
        }

        public async Task<List<Car>> GetAllAsync() => await _repository.GetAllAsync();

        public async Task<Car> GetByIdAsync(Guid id) => await _repository.GetByIdAsync(id);



        public async Task<PaginatedList<Car>>CarPagination(PagedRequest<object?> pagedRequest)
        {


              var query=await _repository.GetCarAllQueryable();

            // Searching 

            // Searching
            if (!string.IsNullOrEmpty(pagedRequest.SearchQuery))
            {
                var searchQuery = pagedRequest.SearchQuery.ToLower();

                // Try parse model number
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
                ? query.OrderByDescending(e => EF.Property<object>(e, pagedRequest.SortColumn))
                    : query.OrderBy(e => EF.Property<object>(e, pagedRequest.SortColumn));
            }
            else
            {
                // Default sorting (optional)
                query = query.OrderBy(x => x.CarId);

            }

            query=query.Skip((pagedRequest.PageIndex-1)*pagedRequest.PageSize).Take(pagedRequest.PageSize);


            var totalrecord=query.Count();
            return new PaginatedList<Car>(query.ToList(), totalRecords, pagedRequest.PageIndex, pagedRequest.PageSize);




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
            
            var data=_repository.GetByIdAsync(car.CarId);

            data.Result.CarId = car.CarId;
            data.Result.Brand = car.Brand;
            data.Result.Activity = car.Activity;
            data.Result.Date = car.Date;
            data.Result.Features = car.Features;
            data.Result.Model = car.Model;
            data.Result.Model_No = car.Model_No;
            data.Result.Price = car.Price;
            data.Result.Classes = car.Classes;
            await _repository.UpdateAsync(data.Result);


        }
        public async Task DeleteAsync(Guid id) => await _repository.DeleteAsync(id);
    }
}
