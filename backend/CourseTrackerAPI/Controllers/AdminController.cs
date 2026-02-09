using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using CourseTrackerAPI.Models;
using CourseTrackerAPI.Repositories;

namespace CourseTrackerAPI.Controllers
{
    [ApiController]
    [Route("admin")]
    [Authorize(Roles = "Admin")]
    public class AdminController : ControllerBase
    {
        private readonly IStudentRepository _repo;

        public AdminController(IStudentRepository repo) => _repo = repo;

        [HttpGet("students")]
        public async Task<IActionResult> GetAll() => Ok(await _repo.GetAll());

        [HttpPut("student/{id}")]
        public async Task<IActionResult> Update(int id, Student updated)
        {
            var existing = await _repo.GetById(id);
            if (existing == null) return NotFound();
            existing.Name = updated.Name;
            existing.Email = updated.Email;
            existing.IndexNumber = updated.IndexNumber;
            return Ok(await _repo.Update(existing));
        }

        [HttpDelete("student/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var success = await _repo.Delete(id);
            return success ? NoContent() : NotFound();
        }
    }
}
