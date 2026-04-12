using HotelBookingManagement.Domain.Entities;
using System;
using System.Threading.Tasks;

namespace HotelBookingManagement.Domain.Interface
{
    public interface ICustomerRepository
    {
        Task<Customer?> GetByIdAsync(Guid id);
        Task<Customer?> GetByEmailAsync(string email);
        Task<Customer?> GetByUsernameAsync(string username);
        Task<Customer?> GetByIdentifierAsync(string identifier);
        Task AddAsync(Customer customer);
        Task UpdateAsync(Customer customer);
    }
}
