using Asp.Versioning;
using CRUDOperations.Db_Context;
using CRUDOperations.DTOs;
using CRUDOperations.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CRUDOperations.Controllers
{
    [ApiController]
    [Asp.Versioning.ApiVersion("1.0")]
    [Asp.Versioning.ApiVersion("2.0")]
    [Asp.Versioning.ApiVersion("3.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class CarController : ControllerBase
    {
        private readonly AppDbContext _appDbContext;

        public CarController(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        /// <summary>
        /// Add a new car (v1 only)
        /// </summary>
        [Authorize]
        [HttpPost("add-car")]
        [Asp.Versioning.MapToApiVersion("1.0")]
        public async Task<IActionResult> AddNewCar([FromBody] AddCarsRequest request)
        {
            try
            {
                if (request == null)
                    return BadRequest("Request is empty");

                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                var car = new Car
                {
                    Brand = request.Brand,
                    Classes = request.Classes,
                    Date = request.Date,
                    Model = request.Model,
                    Model_No = request.Model_No,
                    Price = request.Price,
                    Activity = request.Activity,
                    Features = request.Features
                };

                await _appDbContext.Cars.AddAsync(car);
                await _appDbContext.SaveChangesAsync();

                return Ok("Car added successfully (v1)");
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
        /// <summary>
        /// Update car (v2 only)
        /// </summary>
        [HttpPut("update-car")]
        [Asp.Versioning.MapToApiVersion("2.0")]
        public async Task<IActionResult> Update([FromBody] UpdateCarsRequest request)
        {
            try
            {
                if (request == null)
                    return BadRequest("Request is empty");

                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                var car = await _appDbContext.Cars.FirstOrDefaultAsync(x => x.CarId == request.CarId);
                if (car == null)
                    return NotFound("Car not found");

                car.Brand = request.Brand;
                car.Classes = request.Classes;
                car.Date = request.Date;
                car.Model = request.Model;
                car.Model_No = request.Model_No;
                car.Price = request.Price;
                car.Activity = request.Activity;
                car.Features = request.Features;

                _appDbContext.Cars.Update(car);
                await _appDbContext.SaveChangesAsync();

                return Ok("Car updated successfully (v2)");
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        /// <summary>
        /// Get all cars (available in all versions)
        /// </summary>
        [HttpGet("get-cars")]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var cars = await _appDbContext.Cars.ToListAsync();
                return Ok(cars);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        /// <summary> 
        /// Get all cars (available in all versions)
        /// </summary>
        /// 

        [HttpGet("get-cars-paged")]

        public async Task<IActionResult> GetAllPagination(int pageIndex, int pageSize, string? search, string? sotringcolumn, bool sortingType = false)
        {
            try
            {
                var cars = _appDbContext.Cars.AsQueryable();

                if (!string.IsNullOrWhiteSpace(search))
                {
                    bool isNumber = int.TryParse(search, out int priceValue);
                    cars = cars.Where(x =>
                        x.Brand.Contains(search) ||
                        x.Classes.Contains(search) ||
                        x.Model.Contains(search) ||
                        (isNumber && x.Price == priceValue));
                }

                if (!string.IsNullOrEmpty(sotringcolumn))
                {
                    cars = !sortingType
                        ? cars.OrderBy(e => EF.Property<object>(e, sotringcolumn))
                        : cars.OrderByDescending(e => EF.Property<object>(e, sotringcolumn));
                }
                else
                {
                    cars = cars.OrderBy(x => x.CarId);
                }

                int totalRecords = await cars.CountAsync();
                var pagedCars = await cars.Skip((pageIndex - 1) * pageSize).Take(pageSize).ToListAsync();
                int totalPages = (int)Math.Ceiling(totalRecords / (double)pageSize);

                var response = new
                {
                    totalRecords,
                    totalPages,
                    cars = pagedCars
                };

                return Ok(response);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpGet("get-car-by-id/{id:guid}")]
        public async Task<IActionResult> GetCar(Guid id)
        {
            try
            {
                var car = await _appDbContext.Cars.FirstOrDefaultAsync(x => x.CarId == id);
                if (car == null)
                    return NotFound("Car not found");

                return Ok(car);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpDelete("delete-car/{id:guid}")]
        public async Task<IActionResult> DeleteCar(Guid id)
        {
            try
            {
                var car = await _appDbContext.Cars.FirstOrDefaultAsync(x => x.CarId == id);
                if (car == null)
                    return NotFound("Car not found");

                _appDbContext.Cars.Remove(car);
                await _appDbContext.SaveChangesAsync();

                return Ok("Car deleted successfully");
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }


      }



    //This is only fo checking Api Versioning
    // Separate controllers for each version (alternative approach)
    [ApiController]
    [Asp.Versioning.ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/products")]
    public class ProductsV1Controller : ControllerBase
    {
        [HttpGet]
        public IActionResult Get() => Ok("Products Version 1.0");
    }

    [ApiController]
    [Asp.Versioning.ApiVersion("2.0")]
    [Route("api/v{version:apiVersion}/products")]
    public class ProductsV2Controller : ControllerBase
    {
        [HttpGet]
        public IActionResult Get() => Ok("Products Version 2.0");
    }

    [ApiController]
    [Asp.Versioning.ApiVersion("3.0")]
    [Route("api/v{version:apiVersion}/products")]
    public class ProductsV3Controller : ControllerBase
    {
        [HttpGet]
        public IActionResult Get() => Ok("Products Version 3.0");
    }
}