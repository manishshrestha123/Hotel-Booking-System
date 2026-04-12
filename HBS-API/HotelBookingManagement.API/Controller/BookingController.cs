using HotelBookingManagement.Application.AppService;
using HotelBookingManagement.Application.DTOs;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace HotelBookingManagement.API.Controller
{
    [ApiController]
    [Route("api/bookings")]
    public class BookingController : ControllerBase
    {
        private readonly BookingAppService _bookingService;

        public BookingController(BookingAppService bookingService)
        {
            _bookingService = bookingService;
        }

        /// <summary>Book a room online</summary>
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateBookingDto dto)
        {
            var booking = await _bookingService.CreateBookingAsync(dto);
            return CreatedAtAction(nameof(GetById), new { id = booking.Id }, booking);
        }

        /// <summary>Get booking confirmation details</summary>
        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var booking = await _bookingService.GetBookingByIdAsync(id);
            if (booking == null) return NotFound(new { message = "Booking not found." });
            return Ok(booking);
        }

        /// <summary>Get all bookings for a customer</summary>
        [HttpGet("customer/{customerId:guid}")]
        public async Task<IActionResult> GetByCustomer(Guid customerId)
        {
            var bookings = await _bookingService.GetBookingsByCustomerAsync(customerId);
            return Ok(bookings);
        }

        /// <summary>Find booking by email or booking id</summary>
        [HttpGet("find")]
        public async Task<IActionResult> Find([FromQuery] string identifier)
        {
            if (string.IsNullOrWhiteSpace(identifier)) 
                return BadRequest(new { message = "Identifier is required." });
                
            var bookings = await _bookingService.FindBookingsAsync(identifier);
            return Ok(bookings);
        }

        /// <summary>Cancel a booking</summary>
        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> Cancel(Guid id)
        {
            await _bookingService.CancelBookingAsync(id);
            return Ok(new { message = "Booking cancelled successfully." });
        }

        /// <summary>Modify booking dates</summary>
        [HttpPut("{id:guid}")]
        public async Task<IActionResult> Modify(Guid id, [FromBody] UpdateBookingDto dto)
        {
            var booking = await _bookingService.ModifyBookingAsync(id, dto);
            return Ok(booking);
        }
    }
}
