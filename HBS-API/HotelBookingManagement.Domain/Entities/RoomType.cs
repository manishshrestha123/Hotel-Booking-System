using System;

namespace HotelBookingManagement.Domain.Entities
{
    public class RoomType
    {
        public Guid Id { get; private set; }
        public string Name { get; private set; }
        public string Description { get; private set; }
        public int MaxGuests { get; private set; }

        protected RoomType() { }

        public RoomType(string name, string description, int maxGuests)
        {
            Id = Guid.NewGuid();
            Name = name;
            Description = description;
            MaxGuests = maxGuests;
        }
    }
}
