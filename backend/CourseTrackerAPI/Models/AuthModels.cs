using System.ComponentModel.DataAnnotations;
using CourseTrackerAPI.DTOs.CourseDTO;

public class UserRegisterDto
{
    [Required]
    public required string Username { get; set; }

    [Required]
    public required string Email { get; set; }

    [Required]
    public required string FirstName { get; set; }

    [Required]
    public required string LastName { get; set; }

    [Required]
    public required string Password { get; set; }

    [Required]
    [RegularExpression("^(Admin|Student)$", ErrorMessage = "Role must be Admin or Student")]
    public required string Role { get; set; }  
}





public class UserLoginDto
{
    [Required]
    public required string Username { get; set; }

    [Required]
    public required string Password { get; set; }
}

public class UserDto
{
    public int Id { get; set; }
    public required string Username { get; set; }
    public required string Email { get; set; }
    public required string FirstName { get; set; }
    public required string LastName { get; set; }
    public required List<CourseDto> EnrolledCourses { get; set; }
}

