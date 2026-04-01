using HotelBookingManagement.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HotelBookingManagement.Domain.Interface
{
    public interface IRoomRepository
    {
        Task<IEnumerable<Room>> GetAllAsync();
        Task<Room?> GetByIdAsync(Guid id);
        Task<IEnumerable<Room>> FilterAsync(decimal? minPrice, decimal? maxPrice, Guid? roomTypeId, int? minGuests);
        Task<IEnumerable<RoomAvailability>> GetAvailabilityAsync(Guid roomId, DateTime checkIn, DateTime checkOut);
        Task<bool> IsAvailableAsync(Guid roomId, DateTime checkIn, DateTime checkOut);
        Task<Room> AddAsync(Room room);
    }
}
