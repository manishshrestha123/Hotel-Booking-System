using System;

namespace HotelBookingManagement.Application.DTOs
{
    public class RoomDto
    {
        public Guid Id { get; set; }
        public string RoomNumber { get; set; }
        public decimal PricePerNight { get; set; }
        public string Status { get; set; }
        public string RoomTypeName { get; set; }
        public int MaxGuests { get; set; }
        public string HotelName { get; set; }
        public string PrimaryImageUrl { get; set; }
    }
}
