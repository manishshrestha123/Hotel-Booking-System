using HotelBookingManagement.Domain.Enums;
using System;

namespace HotelBookingManagement.Domain.Entities
{
    public class Payment
    {
        public Guid Id { get; private set; }
        public Guid BookingId { get; private set; }
        public decimal Amount { get; private set; }
        public string PaymentMethod { get; private set; }
        public PaymentStatus PaymentStatus { get; private set; }
        public string TransactionId { get; private set; }
        public DateTime CreatedAt { get; private set; }

        public Booking Booking { get; private set; }

        protected Payment() { }

        public Payment(Guid bookingId, decimal amount, string paymentMethod, PaymentStatus paymentStatus, string transactionId)
        {
            Id = Guid.NewGuid();
            BookingId = bookingId;
            Amount = amount;
            PaymentMethod = paymentMethod;
            PaymentStatus = paymentStatus;
            TransactionId = transactionId;
            CreatedAt = DateTime.UtcNow;
        }
        
        public void UpdateStatus(PaymentStatus status)
        {
            PaymentStatus = status;
        }
    }
}
