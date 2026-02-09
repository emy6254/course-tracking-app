namespace CourseTrackerAPI.Repositories
{
    using CourseTrackerAPI.Data;
    using CourseTrackerAPI.Models;
    using Microsoft.EntityFrameworkCore;

    public class StudentRepository : IStudentRepository
    {
        private readonly DataContext _context;
        public StudentRepository(DataContext context) => _context = context;

        public async Task<IEnumerable<Student>> GetAll() => await _context.Students.ToListAsync();

        public async Task<Student?> GetById(int id) => await _context.Students.FindAsync(id);

        public async Task<Student> Update(Student student)
        {
            _context.Students.Update(student);
            await _context.SaveChangesAsync();
            return student;
        }

        public async Task<bool> Delete(int id)
        {
            var student = await _context.Students.FindAsync(id);
            if (student == null) return false;
            _context.Students.Remove(student);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
