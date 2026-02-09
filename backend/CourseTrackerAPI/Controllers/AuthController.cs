using CourseTrackerAPI.DTOs;
using CourseTrackerAPI.Models;
using CourseTrackerAPI.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

[ApiController]
[Route("api/auth")]
public class AuthController : ControllerBase
{
    private readonly IAuthRepository _auth;
    private readonly IConfiguration _config;

    public AuthController(IAuthRepository auth, IConfiguration config)
    {
        _auth = auth;
        _config = config;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] UserRegisterDto dto)
    {
        if (await _auth.UserExists(dto.Username))
            return BadRequest(new { message = "User already exists" });

        var user = await _auth.Register(dto.Username, dto.Password, dto.Role, dto.Email, dto.FirstName, dto.LastName);
        return Ok(new { message = "Registration successful", userId = user.Id });
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] UserLoginDto dto)
    {
        var user = await _auth.Login(dto.Username, dto.Password);
        if (user == null)
            return Unauthorized(new { message = "Invalid credentials" });

        var claims = new[]
        {
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(ClaimTypes.Name, user.Username),
            new Claim(ClaimTypes.Role, user.Role)
        };

        // Uzmi JWT key iz postojeće konfiguracije
        var jwtKey = _config["AppSettings:Token"] ?? throw new InvalidOperationException("JWT key not configured");
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);
        var token = new JwtSecurityToken(
            claims: claims,
            expires: DateTime.UtcNow.AddHours(1),
            signingCredentials: creds
        );

        return Ok(new
        {
            token = new JwtSecurityTokenHandler().WriteToken(token),
            user = new
            {
                user.Id,
                user.Username,
                user.Email,
                user.FirstName,
                user.LastName,
                user.Role
            }
        });
    }
}