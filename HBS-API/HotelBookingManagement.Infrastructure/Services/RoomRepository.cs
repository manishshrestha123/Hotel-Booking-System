using HotelBookingManagement.Domain.Entities;
using HotelBookingManagement.Domain.Interface;
using HotelBookingManagement.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HotelBookingManagement.Infrastructure.Services
{
    public class RoomRepository : IRoomRepository
    {
        private readonly HotelBookingDbContext _context;

        public RoomRepository(HotelBookingDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Room>> GetAllAsync()
        {
            return await _context.Rooms
                .Include(r => r.Hotel)
                .Include(r => r.RoomType)
                .Include(r => r.Images)
                .ToListAsync();
        }

        public async Task<Room?> GetByIdAsync(Guid id)
        {
            return await _context.Rooms
                .Include(r => r.Hotel)
                .Include(r => r.RoomType)
                .Include(r => r.Images)
                .Include(r => r.Availabilities)
                .FirstOrDefaultAsync(r => r.Id == id);
        }

        public async Task<IEnumerable<Room>> FilterAsync(decimal? minPrice, decimal? maxPrice, Guid? roomTypeId, int? minGuests)
        {
            var query = _context.Rooms
                .Include(r => r.Hotel)
                .Include(r => r.RoomType)
                .Include(r => r.Images)
                .AsQueryable();

            if (minPrice.HasValue)
                query = query.Where(r => r.PricePerNight >= minPrice.Value);

            if (maxPrice.HasValue)
                query = query.Where(r => r.PricePerNight <= maxPrice.Value);

            if (roomTypeId.HasValue)
                query = query.Where(r => r.RoomTypeId == roomTypeId.Value);

            if (minGuests.HasValue)
                query = query.Where(r => r.RoomType.MaxGuests >= minGuests.Value);

            return await query.ToListAsync();
        }

        public async Task<IEnumerable<RoomAvailability>> GetAvailabilityAsync(Guid roomId, DateTime checkIn, DateTime checkOut)
        {
            return await _context.RoomAvailabilities
                .Where(a => a.RoomId == roomId && a.Date >= checkIn && a.Date < checkOut)
                .ToListAsync();
        }

        public async Task<bool> IsAvailableAsync(Guid roomId, DateTime checkIn, DateTime checkOut)
        {
            // Check if any BookingRoom links this room to an overlapping confirmed booking
            bool conflictingBooking = await _context.BookingRooms
                .Include(br => br.Booking)
                .AnyAsync(br =>
                    br.RoomId == roomId &&
                    br.Booking.Status != Domain.Enums.BookingStatus.Cancelled &&
                    br.Booking.CheckInDate < checkOut &&
                    br.Booking.CheckOutDate > checkIn);

            return !conflictingBooking;
        }
    }
}
