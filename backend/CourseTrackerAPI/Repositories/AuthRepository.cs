using CourseTrackerAPI.Data;
using CourseTrackerAPI.Models;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using System.Text;

namespace CourseTrackerAPI.Repositories
{
    public class AuthRepository : IAuthRepository
    {
        private readonly DataContext _context;
        private readonly IConfiguration _config;

        public AuthRepository(DataContext context, IConfiguration config)
        {
            _context = context;
            _config = config;
        }

        public async Task<User?> Login(string username, string password)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Username == username);
            if (user == null)
                return null;

            var hashKey = _config["AppSettings:Token"] ?? throw new InvalidOperationException("Token not configured");
            var key = Encoding.UTF8.GetBytes(hashKey);

            using var hmac = new HMACSHA512(key);
            var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));

            return computedHash.SequenceEqual(user.PasswordHash) ? user : null;
        }

        public async Task<User> Register(string username, string password, string role, string email, string firstName, string lastName)
        {
            var hashKey = _config["AppSettings:Token"] ?? throw new InvalidOperationException("Token not configured");
            var key = Encoding.UTF8.GetBytes(hashKey);

            using var hmac = new HMACSHA512(key);
            var passwordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));

            var user = new User
            {
                Username = username,
                PasswordHash = passwordHash,
                Role = role,
                Email = email,
                FirstName = firstName,
                LastName = lastName,
                CreatedAt = DateTime.UtcNow,
                IsActive = true
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return user;
        }

        public async Task<bool> UserExists(string username)
        {
            return await _context.Users.AnyAsync(u => u.Username == username);
        }
    }
}
