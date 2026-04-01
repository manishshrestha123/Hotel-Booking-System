using HotelBookingManagement.Domain.Enums;
using System;
using System.ComponentModel.DataAnnotations;

namespace HotelBookingManagement.Application.DTOs
{
    public class CreateRoomDto
    {
        [Required]
        public Guid HotelId { get; set; }

        [Required]
        public Guid RoomTypeId { get; set; }

        [Required]
        public string RoomNumber { get; set; }

        [Required]
        [Range(0, double.MaxValue)]
        public decimal PricePerNight { get; set; }

        public RoomStatus Status { get; set; } = RoomStatus.Available;
    }
}
