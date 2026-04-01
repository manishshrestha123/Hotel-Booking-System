using HotelBookingManagement.Application.AppService;
using HotelBookingManagement.Application.DTOs;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace HotelBookingManagement.API.Controller
{
    [ApiController]
    [Route("api/rooms")]
    public class RoomController : ControllerBase
    {
        private readonly RoomAppService _roomService;

        public RoomController(RoomAppService roomService)
        {
            _roomService = roomService;
        }

        /// <summary>Browse all available rooms</summary>
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var rooms = await _roomService.GetAllRoomsAsync();
            return Ok(rooms);
        }

        /// <summary>View room details (images, amenities, policies)</summary>
        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var room = await _roomService.GetRoomByIdAsync(id);
            if (room == null) return NotFound(new { message = "Room not found." });
            return Ok(room);
        }

        /// <summary>Filter rooms by price, type, capacity</summary>
        [HttpGet("filter")]
        public async Task<IActionResult> Filter([FromQuery] RoomFilterDto filter)
        {
            var rooms = await _roomService.FilterRoomsAsync(filter);
            return Ok(rooms);
        }

        /// <summary>Check room availability by date range</summary>
        [HttpGet("{id:guid}/availability")]
        public async Task<IActionResult> CheckAvailability(Guid id, [FromQuery] DateTime checkIn, [FromQuery] DateTime checkOut)
        {
            if (checkOut <= checkIn)
                return BadRequest(new { message = "Check-out must be after check-in." });

            var availability = await _roomService.CheckAvailabilityAsync(id, checkIn, checkOut);
            return Ok(availability);
        }

        /// <summary>Create a new room (admin only)</summary>
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateRoomDto dto)
        {
            var room = await _roomService.CreateRoomAsync(dto);
            return CreatedAtAction(nameof(GetById), new { id = room.Id }, room);
        }
    }
}
