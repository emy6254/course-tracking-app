using CourseTrackerAPI.Models;

namespace CourseTrackerAPI.Repositories
{
    public interface IAuthRepository
    {
        Task<User?> Login(string username, string password);
        Task<User> Register(string username, string password, string role, string email, string firstName, string lastName);
        Task<bool> UserExists(string username);
    }
}
