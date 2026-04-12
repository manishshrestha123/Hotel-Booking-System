using HotelBookingManagement.Domain.Entities;
using HotelBookingManagement.Domain.Interface;
using HotelBookingManagement.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace HotelBookingManagement.Infrastructure.Services
{
    public class UserRepository : IUserRepository
    {
        private readonly HotelBookingDbContext _context;

        public UserRepository(HotelBookingDbContext context)
        {
            _context = context;
        }

        public async Task<User?> GetByEmailAsync(string email)
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
        }

        public async Task<User?> GetByUsernameAsync(string username)
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.Username == username);
        }

        public async Task<User?> GetByIdentifierAsync(string identifier)
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.Email == identifier || u.Username == identifier);
        }

        public async Task AddAsync(User user)
        {
            _context.Users.Add(user);
            await _context.SaveChangesAsync();
        }
    }
}
