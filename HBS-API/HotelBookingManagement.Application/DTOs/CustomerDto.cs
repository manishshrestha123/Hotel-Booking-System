using System;

namespace HotelBookingManagement.Application.DTOs
{
    public class CustomerDto
    {
        public Guid Id { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public string? Username { get; set; }
        public bool HasAccount { get; set; }
    }
}
