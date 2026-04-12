using HotelBookingManagement.Application.AppService;
using HotelBookingManagement.Application.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HotelBookingManagement.API.Controller
{
    [ApiController]
    [Route("api/customers")]
    public class CustomerController : ControllerBase
    {
        private readonly CustomerAppService _customerService;

        public CustomerController(CustomerAppService customerService)
        {
            _customerService = customerService;
        }

        /// <summary>Create a guest customer profile for booking flow</summary>
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateCustomerDto dto)
        {
            try
            {
                var customer = await _customerService.CreateCustomerAsync(dto);
                return CreatedAtAction(nameof(GetById), new { id = customer.Id }, customer);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        /// <summary>Create a customer account with login credentials</summary>
        [Authorize(Roles = "SuperAdmin,Admin,Staff")]
        [HttpPost("account")]
        public async Task<IActionResult> CreateAccount([FromBody] CreateCustomerAccountDto dto)
        {
            try
            {
                var customer = await _customerService.CreateCustomerAccountAsync(dto);
                return CreatedAtAction(nameof(GetById), new { id = customer.Id }, customer);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var customer = await _customerService.GetCustomerByIdAsync(id);
            if (customer == null) return NotFound(new { message = "Customer not found." });
            return Ok(customer);
        }
    }
}
