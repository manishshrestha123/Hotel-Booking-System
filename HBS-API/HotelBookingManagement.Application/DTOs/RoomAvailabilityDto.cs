using System;

namespace HotelBookingManagement.Application.DTOs
{
    public class RoomAvailabilityDto
    {
        public DateTime Date { get; set; }
        public bool IsAvailable { get; set; }
        public decimal? PriceOverride { get; set; }
    }
}
