using System;
using System.Collections.Generic;

namespace HotelBookingManagement.Application.DTOs
{
    public class CreateBookingDto
    {
        public Guid CustomerId { get; set; }
        public Guid HotelId { get; set; }
        public DateTime CheckInDate { get; set; }
        public DateTime CheckOutDate { get; set; }
        public List<Guid> RoomIds { get; set; } = new();
    }
}
