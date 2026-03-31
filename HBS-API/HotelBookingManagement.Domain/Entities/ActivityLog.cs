using HotelBookingManagement.Domain.Enums;
using System;

namespace HotelBookingManagement.Domain.Entities
{
    public class ActivityLog
    {
        public Guid Id { get; private set; }
        public ActivityAction Action { get; private set; }
        public Guid? UserId { get; private set; }
        public DateTime Timestamp { get; private set; }
        public string Description { get; private set; }

        protected ActivityLog() { }

        public ActivityLog(ActivityAction action, Guid? userId, string description)
        {
            Id = Guid.NewGuid();
            Action = action;
            UserId = userId;
            Description = description;
            Timestamp = DateTime.UtcNow;
        }
    }
}
