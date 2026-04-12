using HotelBookingManagement.Domain.Enums;

namespace HotelBookingManagement.Domain.Entities
{
    public class User
    {
        public Guid Id { get; private set; }
        public string Username { get; private set; }
        public string Email { get; private set; }
        public string PasswordHash { get; private set; }
        public string FullName { get; private set; }
        public UserRole Role { get; private set; }

        protected User() { }

        public User(string username, string email, string passwordHash, string fullName, UserRole role)
        {
            Id = Guid.NewGuid();
            Username = username;
            Email = email;
            PasswordHash = passwordHash;
            FullName = fullName;
            Role = role;
        }
    }
}
