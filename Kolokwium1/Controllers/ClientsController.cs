using Kolokwium1.Exceptions;
using Kolokwium1.Models.DTOs;
using Kolokwium1.Services;
using Microsoft.AspNetCore.Mvc;

namespace Kolokwium1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomersController : ControllerBase
    {
        private readonly IDbService _dbService;
        public CustomersController(IDbService dbService)
        {
            _dbService = dbService;
        }

        [HttpGet("clients/{id}")]
        public async Task<IActionResult> GetCustomerRentals(int id)
        {
            try
            {
                var res = await _dbService.GetRentalsForCustomerByIdAsync(id);
                return Ok(res);
            }
            catch (NotFoundException e)
            {
                return NotFound(e.Message);
            }
        }

        [HttpPost("clients/{id}")]
        public async Task<IActionResult> AddNewRental(int id, CreateRentalDto createRentalRequest)
        {

            try
            {
                await _dbService.AddNewRentalAsync(id, createRentalRequest);
            }
            catch (ConflictException e)
            {
                return Conflict(e.Message);
            }
            catch (NotFoundException e)
            {
                return NotFound(e.Message);
            }
            
            return CreatedAtAction(nameof(GetCustomerRentals), new { id }, createRentalRequest);
        }    
    }
}