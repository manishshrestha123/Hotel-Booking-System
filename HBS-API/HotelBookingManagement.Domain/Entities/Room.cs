using HotelBookingManagement.Domain.Enums;
using System;
using System.Collections.Generic;

namespace HotelBookingManagement.Domain.Entities
{
    public class Room
    {
        public Guid Id { get; private set; }
        public Guid HotelId { get; private set; }
        public Guid RoomTypeId { get; private set; }
        public string RoomNumber { get; private set; }
        public decimal PricePerNight { get; private set; }
        public RoomStatus Status { get; private set; }

        public Hotel Hotel { get; private set; }
        public RoomType RoomType { get; private set; }
        public ICollection<RoomImage> Images { get; private set; }
        public ICollection<RoomAvailability> Availabilities { get; private set; }

        protected Room()
        {
            Images = new List<RoomImage>();
            Availabilities = new List<RoomAvailability>();
        }

        public Room(Guid hotelId, Guid roomTypeId, string roomNumber, decimal pricePerNight, RoomStatus status)
        {
            Id = Guid.NewGuid();
            HotelId = hotelId;
            RoomTypeId = roomTypeId;
            RoomNumber = roomNumber;
            PricePerNight = pricePerNight;
            Status = status;
            Images = new List<RoomImage>();
            Availabilities = new List<RoomAvailability>();
        }

        public void SetStatus(RoomStatus status)
        {
            Status = status;
        }
    }
}
