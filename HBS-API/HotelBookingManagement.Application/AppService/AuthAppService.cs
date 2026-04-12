using HotelBookingManagement.Application.DTOs;
using HotelBookingManagement.Domain.Entities;
using HotelBookingManagement.Domain.Interface;

namespace HotelBookingManagement.Application.AppService
{
    public class AuthAppService
    {
        private readonly IUserRepository _userRepository;
        private readonly ICustomerRepository _customerRepository;
        private readonly IAuthService _authService;

        public AuthAppService(
            IUserRepository userRepository,
            ICustomerRepository customerRepository,
            IAuthService authService)
        {
            _userRepository = userRepository;
            _customerRepository = customerRepository;
            _authService = authService;
        }

        public async Task CreateUserAsync(CreateUserDto dto)
        {
            var existingUserByEmail = await _userRepository.GetByEmailAsync(dto.Email);
            if (existingUserByEmail != null)
                throw new Exception("Email already registered.");

            var existingUserByUsername = await _userRepository.GetByUsernameAsync(dto.Username);
            if (existingUserByUsername != null)
                throw new Exception("Username already registered.");

            var hash = _authService.HashPassword(dto.Password);
            var user = new User(dto.Username, dto.Email, hash, dto.FullName, dto.Role);

            await _userRepository.AddAsync(user);
        }

        public async Task<AuthResponseDto> LoginAsync(LoginUserDto dto)
        {
            var user = await _userRepository.GetByIdentifierAsync(dto.Identifier);
            if (user == null || !_authService.VerifyPassword(user.PasswordHash, dto.Password))
                throw new Exception("Invalid username/email or password.");

            var token = _authService.GenerateJwtToken(user);

            return new AuthResponseDto
            {
                Id = user.Id,
                Token = token,
                Username = user.Username,
                Email = user.Email,
                Role = user.Role.ToString()
            };
        }

        public async Task<AuthResponseDto> CustomerLoginAsync(CustomerLoginDto dto)
        {
            var customer = await _customerRepository.GetByIdentifierAsync(dto.Identifier);
            if (customer == null || string.IsNullOrWhiteSpace(customer.PasswordHash))
                throw new Exception("Invalid username/email or password.");

            if (!_authService.VerifyPassword(customer.PasswordHash, dto.Password))
                throw new Exception("Invalid username/email or password.");

            var token = _authService.GenerateCustomerJwtToken(customer);

            return new AuthResponseDto
            {
                Id = customer.Id,
                Token = token,
                Username = customer.Username ?? string.Empty,
                Email = customer.Email,
                Role = "Customer"
            };
        }
    }
}
