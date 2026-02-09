using AutoMapper;
using CourseTrackerAPI.DTOs.CourseDTO;
using CourseTrackerAPI.Models;
using CourseTrackerAPI.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace CourseTrackerAPI.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class CoursesController : ControllerBase
    {
        private readonly ICourseRepository _repo;
        private readonly IMapper _mapper;
        private readonly ILogger<CoursesController> _logger;

        public CoursesController(ICourseRepository repo, IMapper mapper, ILogger<CoursesController> logger)
        {
            _repo = repo;
            _mapper = mapper;
            _logger = logger;
        }

        [Authorize(Roles = "Admin,Student")]
        [HttpGet]
        public async Task<IActionResult> GetCourses([FromQuery] int page = 1, [FromQuery] int pageSize = 10)
        {
            try
            {
                if (page < 1) page = 1;
                if (pageSize < 1 || pageSize > 100) pageSize = 10;

                var skip = (page - 1) * pageSize;
                var totalCount = await _repo.GetCoursesCount();
                var courses = await _repo.GetCourse(skip, pageSize);

                var coursesToReturn = _mapper.Map<List<CourseDto>>(courses);

                // ✅ Dodajemo broj prijavljenih korisnika po kursu
                foreach (var dto in coursesToReturn)
                {
                    dto.EnrolledCount = await _repo.GetEnrollmentCount(dto.Id);
                }

                var response = new
                {
                    courses = coursesToReturn,
                    pagination = new
                    {
                        currentPage = page,
                        pageSize = pageSize,
                        totalCount = totalCount,
                        totalPages = (int)Math.Ceiling((double)totalCount / pageSize)
                    }
                };

                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while retrieving courses");
                return StatusCode(500, new { message = "An error occurred while retrieving courses" });
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetCourse(int id)
        {
            var course = await _repo.GetCourse(id);
            if (course == null)
                return NotFound(new { message = "Course not found" });

            var courseToReturn = _mapper.Map<CourseDto>(course);
            courseToReturn.EnrolledCount = await _repo.GetEnrollmentCount(course.Id);

            return Ok(courseToReturn);
        }

        [HttpPost("enroll/{courseId}")]
        public async Task<IActionResult> EnrollInCourse(int courseId)
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
            if (userIdClaim == null || !int.TryParse(userIdClaim.Value, out int userId))
                return Unauthorized(new { message = "Invalid user token" });

            var course = await _repo.GetCourse(courseId);
            if (course == null)
                return NotFound(new { message = "Course not found" });

            var isAlreadyEnrolled = await _repo.IsUserEnrolledInCourse(userId, courseId);
            if (isAlreadyEnrolled)
                return BadRequest(new { message = "Already enrolled" });

            await _repo.EnrollUserInCourse(userId, courseId);

            var dto = _mapper.Map<CourseDto>(course);
            dto.EnrolledCount = await _repo.GetEnrollmentCount(course.Id);

            return Ok(new
            {
                message = "Enrolled successfully",
                course = dto
            });
        }

        [HttpDelete("unenroll/{courseId}")]
        public async Task<IActionResult> UnenrollFromCourse(int courseId)
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
            if (userIdClaim == null || !int.TryParse(userIdClaim.Value, out int userId))
                return Unauthorized(new { message = "Invalid user token" });

            var isEnrolled = await _repo.IsUserEnrolledInCourse(userId, courseId);
            if (!isEnrolled)
                return BadRequest(new { message = "You are not enrolled in this course" });

            await _repo.UnenrollUserFromCourse(userId, courseId);
            return Ok(new { message = "Unenrolled successfully" });
        }

        [HttpGet("user")]
        public async Task<IActionResult> GetUserCourses()
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
            if (userIdClaim == null || !int.TryParse(userIdClaim.Value, out int userId))
                return Unauthorized(new { message = "Invalid user token" });

            var courses = await _repo.GetUserCourses(userId);
            var coursesToReturn = _mapper.Map<IEnumerable<CourseDto>>(courses);

            foreach (var course in coursesToReturn)
            {
                course.EnrolledCount = await _repo.GetEnrollmentCount(course.Id);
            }

            return Ok(coursesToReturn);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> CreateCourse([FromBody] CourseCreateDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var course = new Course
            {
                Title = dto.Title,
                Description = dto.Description,
                Instructor = dto.Instructor,
                Price = dto.Price ?? 0,
                Duration = dto.Duration,
                Level = dto.Level,
                Category = dto.Category,
                ImageUrl = dto.ImageUrl,
                CreatedAt = DateTime.UtcNow
            };

            await _repo.AddCourse(course);
            return Ok(new { message = "Kurs uspešno kreiran", courseId = course.Id });
        }
    }
}
