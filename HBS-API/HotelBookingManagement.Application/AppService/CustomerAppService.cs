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
        private readonly IAuthService _authService;

        public CustomerAppService(ICustomerRepository customerRepository, IAuthService authService)
        {
            _customerRepository = customerRepository;
            _authService = authService;
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
                DateOfBirth = customer.DateOfBirth,
                Username = customer.Username,
                HasAccount = !string.IsNullOrWhiteSpace(customer.PasswordHash)
            };
        }

        public async Task<CustomerDto> CreateCustomerAccountAsync(CreateCustomerAccountDto dto)
        {
            var existingByEmail = await _customerRepository.GetByEmailAsync(dto.Email);
            if (existingByEmail != null)
                throw new Exception("A customer with this email already exists.");

            var existingByUsername = await _customerRepository.GetByUsernameAsync(dto.Username);
            if (existingByUsername != null)
                throw new Exception("A customer with this username already exists.");

            var hash = _authService.HashPassword(dto.Password);
            var customer = new Customer(dto.FullName, dto.Email, dto.Phone, dto.Username, hash, dto.DateOfBirth);

            await _customerRepository.AddAsync(customer);

            return new CustomerDto
            {
                Id = customer.Id,
                FullName = customer.FullName,
                Email = customer.Email,
                Phone = customer.Phone,
                DateOfBirth = customer.DateOfBirth,
                Username = customer.Username,
                HasAccount = true
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
                DateOfBirth = c.DateOfBirth,
                Username = c.Username,
                HasAccount = !string.IsNullOrWhiteSpace(c.PasswordHash)
            };
        }
    }
}
