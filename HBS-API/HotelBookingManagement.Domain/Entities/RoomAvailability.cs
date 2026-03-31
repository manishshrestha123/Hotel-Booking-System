using System;

namespace HotelBookingManagement.Domain.Entities
{
    public class RoomAvailability
    {
        public Guid Id { get; private set; }
        public Guid RoomId { get; private set; }
        public DateTime Date { get; private set; }
        public bool IsAvailable { get; private set; }
        public decimal? PriceOverride { get; private set; }

        public Room Room { get; private set; }

        protected RoomAvailability() { }

        public RoomAvailability(Guid roomId, DateTime date, bool isAvailable, decimal? priceOverride = null)
        {
            Id = Guid.NewGuid();
            RoomId = roomId;
            Date = date;
            IsAvailable = isAvailable;
            PriceOverride = priceOverride;
        }

        public void UpdateAvailability(bool isAvailable, decimal? priceOverride = null)
        {
            IsAvailable = isAvailable;
            PriceOverride = priceOverride;
        }
    }
}
