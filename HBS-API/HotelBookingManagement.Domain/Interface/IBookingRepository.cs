using HotelBookingManagement.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HotelBookingManagement.Domain.Interface
{
    public interface IBookingRepository
    {
        Task<Booking?> GetByIdAsync(Guid id);
        Task<IEnumerable<Booking>> GetByCustomerIdAsync(Guid customerId);
        Task AddAsync(Booking booking);
        Task UpdateAsync(Booking booking);
    }
}
