using HotelBookingManagement.Domain.Entities;
using HotelBookingManagement.Domain.Interface;
using HotelBookingManagement.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace HotelBookingManagement.Infrastructure.Services
{
    public class CustomerRepository : ICustomerRepository
    {
        private readonly HotelBookingDbContext _context;

        public CustomerRepository(HotelBookingDbContext context)
        {
            _context = context;
        }

        public async Task<Customer?> GetByIdAsync(Guid id)
        {
            return await _context.Customers.FirstOrDefaultAsync(c => c.Id == id);
        }

        public async Task<Customer?> GetByEmailAsync(string email)
        {
            return await _context.Customers.FirstOrDefaultAsync(c => c.Email == email);
        }

        public async Task<Customer?> GetByUsernameAsync(string username)
        {
            return await _context.Customers.FirstOrDefaultAsync(c => c.Username == username);
        }

        public async Task<Customer?> GetByIdentifierAsync(string identifier)
        {
            return await _context.Customers.FirstOrDefaultAsync(c => c.Email == identifier || c.Username == identifier);
        }

        public async Task AddAsync(Customer customer)
        {
            await _context.Customers.AddAsync(customer);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Customer customer)
        {
            _context.Customers.Update(customer);
            await _context.SaveChangesAsync();
        }
    }
}
