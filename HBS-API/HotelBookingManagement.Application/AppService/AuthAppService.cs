using HotelBookingManagement.Application.DTOs;
using HotelBookingManagement.Domain.Entities;
using HotelBookingManagement.Domain.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelBookingManagement.Application.AppService
{
    public class AuthAppService
    {
        private readonly IUserRepository _userRepository;
        private readonly IAuthService _authService;

        public AuthAppService(IUserRepository userRepository, IAuthService authService)
        {
            _userRepository = userRepository;
            _authService = authService;
        }

        public async Task RegisterAsync(RegisterUserDto dto)
        {
            var existingUser = await _userRepository.GetByEmailAsync(dto.Email);
            if (existingUser != null)
                throw new Exception("Email already registered");

            var hash = _authService.HashPassword(dto.Password);
            var user = new User(dto.Username, dto.Email, hash, dto.FullName);

            await _userRepository.AddAsync(user);
        }

        public async Task<AuthResponseDto> LoginAsync(LoginUserDto dto)
        {
            var user = await _userRepository.GetByEmailAsync(dto.Email);
            if (user == null || !_authService.VerifyPassword(user.PasswordHash, dto.Password))
                throw new Exception("Invalid email or password");

            var token = _authService.GenerateJwtToken(user);

            return new AuthResponseDto
            {
                Token = token,
                Username = user.Username,
                Email = user.Email
            };
        }
    }
}
