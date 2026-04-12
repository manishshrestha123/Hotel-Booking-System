using System;
using System.Collections.Generic;

namespace HotelBookingManagement.Domain.Entities
{
    public class Customer
    {
        public Guid Id { get; private set; }
        public string FullName { get; private set; }
        public string Email { get; private set; }
        public string Phone { get; private set; }
        public DateTime? DateOfBirth { get; private set; }

        public ICollection<Booking> Bookings { get; private set; }

        protected Customer()
        {
            Bookings = new List<Booking>();
        }

        public Customer(string fullName, string email, string phone)
        {
            Id = Guid.NewGuid();
            FullName = fullName;
            Email = email;
            Phone = phone;
            DateOfBirth = null; // Optional initially unless passed
            Bookings = new List<Booking>();
        }
        // Overload to support DateOfBirth
        public Customer(string fullName, string email, string phone, DateTime dob) : this(fullName, email, phone)
        {
            DateOfBirth = dob;
        }
    }
}
