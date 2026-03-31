using System;

namespace HotelBookingManagement.Domain.Entities
{
    public class RoomImage
    {
        public Guid Id { get; private set; }
        public Guid RoomId { get; private set; }
        public string ImageUrl { get; private set; }
        public bool IsPrimary { get; private set; }

        public Room Room { get; private set; }

        protected RoomImage() { }

        public RoomImage(Guid roomId, string imageUrl, bool isPrimary)
        {
            Id = Guid.NewGuid();
            RoomId = roomId;
            ImageUrl = imageUrl;
            IsPrimary = isPrimary;
        }

        public void SetPrimary(bool isPrimary)
        {
            IsPrimary = isPrimary;
        }
    }
}
