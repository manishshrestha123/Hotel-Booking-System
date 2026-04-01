using System;
using System.Collections.Generic;

namespace HotelBookingManagement.Application.DTOs
{
    public class BookingDto
    {
        public Guid Id { get; set; }
        public Guid CustomerId { get; set; }
        public string HotelName { get; set; }
        public DateTime CheckInDate { get; set; }
        public DateTime CheckOutDate { get; set; }
        public decimal TotalAmount { get; set; }
        public string Status { get; set; }
        public DateTime CreatedAt { get; set; }
        public List<string> RoomNumbers { get; set; } = new();
    }
}
