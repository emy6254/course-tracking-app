using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using CourseTrackerAPI.Data;
using CourseTrackerAPI.Models;

namespace CourseTrackerAPI.Controllers
{
    [ApiController]
    [Route("profile")]
    [Authorize(Roles = "student")]
    public class ProfileController : ControllerBase
    {
        private readonly DataContext _context;

        public ProfileController(DataContext context) => _context = context;

        [HttpGet]
        public IActionResult GetProfile()
        {
            var username = User.Identity?.Name;
            var user = _context.Users.FirstOrDefault(u => u.Username == username);
            if (user == null) return NotFound();
            return Ok(user);
        }
    }
}