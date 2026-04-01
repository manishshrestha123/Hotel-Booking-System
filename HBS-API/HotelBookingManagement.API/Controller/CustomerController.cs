using HotelBookingManagement.Application.AppService;
using HotelBookingManagement.Application.DTOs;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

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

        /// <summary>Register a new guest/customer</summary>
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateCustomerDto dto)
        {
            var customer = await _customerService.CreateCustomerAsync(dto);
            return CreatedAtAction(nameof(GetById), new { id = customer.Id }, customer);
        }

        /// <summary>Get customer profile</summary>
        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var customer = await _customerService.GetCustomerByIdAsync(id);
            if (customer == null) return NotFound(new { message = "Customer not found." });
            return Ok(customer);
        }
    }
}
