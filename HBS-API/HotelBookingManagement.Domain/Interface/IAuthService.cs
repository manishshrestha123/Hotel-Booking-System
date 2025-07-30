using HotelBookingManagement.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelBookingManagement.Domain.Interface
{
    public interface IAuthService
    {
        string HashPassword(string password);
        bool VerifyPassword(string hash, string password);
        string GenerateJwtToken(User user);
    }
}
