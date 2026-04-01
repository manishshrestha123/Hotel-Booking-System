using System;

namespace HotelBookingManagement.Application.DTOs
{
    public class RoomFilterDto
    {
        public decimal? MinPrice { get; set; }
        public decimal? MaxPrice { get; set; }
        public Guid? RoomTypeId { get; set; }
        public int? MinGuests { get; set; }
    }
}
