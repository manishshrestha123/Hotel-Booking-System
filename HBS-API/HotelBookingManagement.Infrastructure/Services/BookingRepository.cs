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
    public class BookingRepository : IBookingRepository
    {
        private readonly HotelBookingDbContext _context;

        public BookingRepository(HotelBookingDbContext context)
        {
            _context = context;
        }

        public async Task<Booking?> GetByIdAsync(Guid id)
        {
            return await _context.Bookings
                .Include(b => b.Customer)
                .Include(b => b.Hotel)
                .Include(b => b.BookingRooms)
                    .ThenInclude(br => br.Room)
                .FirstOrDefaultAsync(b => b.Id == id);
        }

        public async Task<IEnumerable<Booking>> GetByCustomerIdAsync(Guid customerId)
        {
            return await _context.Bookings
                .Include(b => b.Hotel)
                .Include(b => b.BookingRooms)
                    .ThenInclude(br => br.Room)
                .Where(b => b.CustomerId == customerId)
                .ToListAsync();
        }

        public async Task AddAsync(Booking booking)
        {
            await _context.Bookings.AddAsync(booking);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Booking booking)
        {
            _context.Bookings.Update(booking);
            await _context.SaveChangesAsync();
        }
    }
}
