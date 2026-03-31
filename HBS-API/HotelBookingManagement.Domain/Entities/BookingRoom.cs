using System;

namespace HotelBookingManagement.Domain.Entities
{
    public class BookingRoom
    {
        public Guid Id { get; private set; }
        public Guid BookingId { get; private set; }
        public Guid RoomId { get; private set; }
        public decimal PricePerNight { get; private set; }

        public Booking Booking { get; private set; }
        public Room Room { get; private set; }

        protected BookingRoom() { }

        public BookingRoom(Guid bookingId, Guid roomId, decimal pricePerNight)
        {
            Id = Guid.NewGuid();
            BookingId = bookingId;
            RoomId = roomId;
            PricePerNight = pricePerNight;
        }
    }
}
