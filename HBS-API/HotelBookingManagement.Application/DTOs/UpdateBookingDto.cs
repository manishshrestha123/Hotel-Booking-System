using System;

namespace HotelBookingManagement.Application.DTOs
{
    public class UpdateBookingDto
    {
        public DateTime CheckInDate { get; set; }
        public DateTime CheckOutDate { get; set; }
    }
}
