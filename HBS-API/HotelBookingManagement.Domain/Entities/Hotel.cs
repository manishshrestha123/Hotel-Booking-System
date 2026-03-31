using System;
using System.Collections.Generic;

namespace HotelBookingManagement.Domain.Entities
{
    public class Hotel
    {
        public Guid Id { get; private set; }
        public string Name { get; private set; }
        public string Address { get; private set; }
        public string City { get; private set; }
        public string Country { get; private set; }
        public string Phone { get; private set; }
        public string Email { get; private set; }
        public string Description { get; private set; }
        public DateTime CreatedAt { get; private set; }

        public ICollection<Room> Rooms { get; private set; }

        protected Hotel()
        {
            Rooms = new List<Room>();
        }

        public Hotel(string name, string address, string city, string country, string phone, string email, string description)
        {
            Id = Guid.NewGuid();
            Name = name;
            Address = address;
            City = city;
            Country = country;
            Phone = phone;
            Email = email;
            Description = description;
            CreatedAt = DateTime.UtcNow;
            Rooms = new List<Room>();
        }
    }
}
