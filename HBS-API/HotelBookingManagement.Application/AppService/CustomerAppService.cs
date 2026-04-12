using HotelBookingManagement.Application.DTOs;
using HotelBookingManagement.Domain.Entities;
using HotelBookingManagement.Domain.Interface;
using System;
using System.Threading.Tasks;

namespace HotelBookingManagement.Application.AppService
{
    public class CustomerAppService
    {
        private readonly ICustomerRepository _customerRepository;

        public CustomerAppService(ICustomerRepository customerRepository)
        {
            _customerRepository = customerRepository;
        }

        public async Task<CustomerDto> CreateCustomerAsync(CreateCustomerDto dto)
        {
            var existing = await _customerRepository.GetByEmailAsync(dto.Email);
            if (existing != null)
                throw new Exception("A customer with this email already exists.");

            Customer customer;
            if (dto.DateOfBirth.HasValue)
                customer = new Customer(dto.FullName, dto.Email, dto.Phone, dto.DateOfBirth.Value);
            else
                customer = new Customer(dto.FullName, dto.Email, dto.Phone);
                
            await _customerRepository.AddAsync(customer);

            return new CustomerDto
            {
                Id = customer.Id,
                FullName = customer.FullName,
                Email = customer.Email,
                Phone = customer.Phone,
                DateOfBirth = customer.DateOfBirth
            };
        }

        public async Task<CustomerDto?> GetCustomerByIdAsync(Guid id)
        {
            var c = await _customerRepository.GetByIdAsync(id);
            if (c == null) return null;

            return new CustomerDto
            {
                Id = c.Id,
                FullName = c.FullName,
                Email = c.Email,
                Phone = c.Phone,
                DateOfBirth = c.DateOfBirth
            };
        }
    }
}
