using Business.Contract.IServices;
using Business.Models;
using Business.Models.Request;
using Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CarController : ControllerBase
    {
        private readonly ICarService _service;

        public CarController(ICarService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll() => Ok(await _service.GetAllAsync());

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var car = await _service.GetByIdAsync(id);
            if (car == null) return NotFound();
            return Ok(car);
        }

        [HttpPost]
        public async Task<IActionResult> Add(AddCarsRequest car)
        {

            if (car == null) return NotFound();

            await _service.AddAsync(car);
            return Ok(car);
        }

        [HttpPut("update")]
        public async Task<IActionResult> Update(UpdateCarsRequest car)
        {
            if (!ModelState.IsValid) return BadRequest();
            await _service.UpdateAsync(car);
            return Ok(car);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            await _service.DeleteAsync(id);
            return Ok("Deleted");
        }
        [HttpPost("paged")]
        public async Task<IActionResult> GeCarPaged([FromForm] PagedRequest<object> pagedRequest)
        {
           var data=  await _service.CarPagination(pagedRequest);

            if(data == null) return NotFound("Data Not Found");
            return Ok(data);
        }
    }
}
