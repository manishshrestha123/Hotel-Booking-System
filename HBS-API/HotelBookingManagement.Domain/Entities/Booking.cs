using HotelBookingManagement.Domain.Enums;
using System;
using System.Collections.Generic;

namespace HotelBookingManagement.Domain.Entities
{
    public class Booking
    {
        public Guid Id { get; private set; }
        public Guid CustomerId { get; private set; }
        public Guid HotelId { get; private set; }
        public DateTime CheckInDate { get; private set; }
        public DateTime CheckOutDate { get; private set; }
        public decimal TotalAmount { get; private set; }
        public BookingStatus Status { get; private set; }
        public DateTime CreatedAt { get; private set; }

        public Customer Customer { get; private set; }
        public Hotel Hotel { get; private set; }
        public ICollection<BookingRoom> BookingRooms { get; private set; }

        protected Booking()
        {
            BookingRooms = new List<BookingRoom>();
        }

        public Booking(Guid customerId, Guid hotelId, DateTime checkInDate, DateTime checkOutDate, decimal totalAmount, BookingStatus status)
        {
            Id = Guid.NewGuid();
            CustomerId = customerId;
            HotelId = hotelId;
            CheckInDate = checkInDate;
            CheckOutDate = checkOutDate;
            TotalAmount = totalAmount;
            Status = status;
            CreatedAt = DateTime.UtcNow;
            BookingRooms = new List<BookingRoom>();
        }

        public void ChangeStatus(BookingStatus status)
        {
            Status = status;
        }
    }
}
