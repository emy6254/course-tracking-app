using AutoMapper;
using CourseTrackerAPI.Data;
using CourseTrackerAPI.DTOs;
using CourseTrackerAPI.DTOs.CourseDTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

[Authorize]
[Route("api/[controller]")]
[ApiController]
public class UsersController : ControllerBase
{
    private readonly DataContext _context;
    private readonly IMapper _mapper;
    private readonly ILogger<UsersController> _logger;
    private readonly IConfiguration _config;

    public UsersController(DataContext context, IMapper mapper, ILogger<UsersController> logger, IConfiguration config)
    {
        _context = context;
        _mapper = mapper;
        _logger = logger;
        _config = config;
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetUser(int id)
    {
        var currentUserId = GetUserId();
        if (currentUserId == null) return Unauthorized(new { message = "Invalid token" });

        var userRole = User.FindFirst(ClaimTypes.Role)?.Value;
        if (currentUserId != id && userRole != "Admin")
            return Forbid("You can only view your own profile");

        var user = await _context.Users
            .Include(u => u.EnrolledCourses).ThenInclude(ec => ec.Course)
            .FirstOrDefaultAsync(u => u.Id == id);

        if (user == null) return NotFound(new { message = "User not found" });

        var userToReturn = _mapper.Map<UserDto>(user);
        return Ok(userToReturn);
    }

    [HttpGet]
    public async Task<IActionResult> GetCurrentUser()
    {
        var userId = GetUserId();
        if (userId == null) return Unauthorized(new { message = "Invalid token" });

        var user = await _context.Users
            .Include(u => u.EnrolledCourses).ThenInclude(ec => ec.Course)
            .FirstOrDefaultAsync(u => u.Id == userId);

        if (user == null) return NotFound(new { message = "User not found" });

        var userToReturn = _mapper.Map<UserDto>(user);
        return Ok(userToReturn);
    }
    [HttpGet("enrollments")]
    public async Task<IActionResult> GetEnrollments()
    {
        var userId = GetUserId();
        if (userId == null) return Unauthorized(new { message = "Invalid token" });

        var user = await _context.Users
            .Include(u => u.EnrolledCourses)
            .ThenInclude(uc => uc.Course)
            .FirstOrDefaultAsync(u => u.Id == userId);

        if (user == null) return NotFound(new { message = "User not found" });

        var courses = user.EnrolledCourses.Select(ec => ec.Course).ToList();
        var courseDtos = _mapper.Map<List<CourseDto>>(courses);

        foreach (var dto in courseDtos)
        {
            dto.EnrolledCount = await _context.UserCourses.CountAsync(uc => uc.CourseId == dto.Id);
        }

        return Ok(courseDtos);
    }


    [HttpPut("profile")]
    public async Task<IActionResult> UpdateProfile(UpdateUserDto dto)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);

        var userId = GetUserId();
        if (userId == null) return Unauthorized(new { message = "Invalid token" });

        var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == userId);
        if (user == null) return NotFound(new { message = "User not found" });

        if (!string.Equals(user.Email, dto.Email, StringComparison.OrdinalIgnoreCase))
        {
            var exists = await _context.Users.AnyAsync(u => u.Email.ToLower() == dto.Email.ToLower() && u.Id != userId);
            if (exists) return BadRequest(new { message = "Email is already taken" });
        }

        user.Email = dto.Email.Trim().ToLower();
        user.FirstName = dto.FirstName.Trim();
        user.LastName = dto.LastName.Trim();

        await _context.SaveChangesAsync();
        var userToReturn = _mapper.Map<UserDto>(user);

        return Ok(new { message = "Profile updated successfully", user = userToReturn });
    }

    [HttpPut("change-password")]
    public async Task<IActionResult> ChangePassword(ChangePasswordDto dto)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);

        var userId = GetUserId();
        if (userId == null) return Unauthorized(new { message = "Invalid token" });

        var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == userId);
        if (user == null) return NotFound(new { message = "User not found" });

        if (!VerifyPasswordHash(dto.CurrentPassword, user.PasswordHash))
            return BadRequest(new { message = "Current password is incorrect" });

        user.PasswordHash = CreatePasswordHash(dto.NewPassword);
        await _context.SaveChangesAsync();

        return Ok(new { message = "Password changed successfully" });
    }

    [HttpDelete]
    public async Task<IActionResult> DeleteAccount()
    {
        var userId = GetUserId();
        if (userId == null) return Unauthorized(new { message = "Invalid token" });

        var user = await _context.Users.Include(u => u.EnrolledCourses).FirstOrDefaultAsync(u => u.Id == userId);
        if (user == null) return NotFound(new { message = "User not found" });

        _context.UserCourses.RemoveRange(user.EnrolledCourses);
        _context.Users.Remove(user);
        await _context.SaveChangesAsync();

        return Ok(new { message = "Account deleted successfully" });
    }

    // Utility methods
    private int? GetUserId()
    {
        var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
        if (userIdClaim == null || !int.TryParse(userIdClaim.Value, out int id)) return null;
        return id;
    }

    private byte[] CreatePasswordHash(string password)
    {
        // Koristi isti JWT ključ za hash (ili možete dodati poseban ključ)
        var hashKey = _config["AppSettings:Token"] ?? throw new InvalidOperationException("Hash key not configured");
        var key = Encoding.UTF8.GetBytes(hashKey);

        using var hmac = new HMACSHA512(key);
        return hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
    }

    private bool VerifyPasswordHash(string password, byte[] storedHash)
    {
        // Koristi isti JWT ključ za verifikaciju
        var hashKey = _config["AppSettings:Token"] ?? throw new InvalidOperationException("Hash key not configured");
        var key = Encoding.UTF8.GetBytes(hashKey);

        using var hmac = new HMACSHA512(key);
        var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
        return computedHash.SequenceEqual(storedHash);
    }
}