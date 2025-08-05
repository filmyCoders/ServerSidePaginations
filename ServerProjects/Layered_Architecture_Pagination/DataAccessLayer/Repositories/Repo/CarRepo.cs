using DataAccess.Db_Context;
using DataAccess.Repositories.IRepo;
using Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace DataAccess.Repositories.Repo
{
    public class CarRepo : ICarRepo
    {
        private readonly AppDbContext _context;

        public CarRepo(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<Car>> GetAllAsync()
        {
            return await _context.Cars.ToListAsync();
        }

        public async Task<Car> GetByIdAsync(Guid id)
        {
            return await _context.Cars.FindAsync(id);
        }

        public async Task AddAsync(Car car)
        {
            await _context.Cars.AddAsync(car);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Car car)
        {
            _context.Cars.Update(car);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Guid id)
        {
            var car = await _context.Cars.FindAsync(id);
            if (car != null)
            {
                _context.Cars.Remove(car);
                await _context.SaveChangesAsync();
            }
        }

        public Task<IQueryable<Car>> GetCarAllQueryable()
        {
            return Task.FromResult(_context.Cars.AsQueryable());
        }

        public Task<IQueryable<Car>> GetCarAllQueryable(Expression<Func<Car, bool>> predicate)
        {
            var cars = predicate == null
                ? _context.Cars.AsQueryable()
                : _context.Cars.Where(predicate).AsQueryable();

            return Task.FromResult(cars);
        }


    }
}
