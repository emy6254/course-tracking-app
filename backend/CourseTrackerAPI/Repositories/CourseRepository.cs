namespace CourseTrackerAPI.Repositories
{
    using CourseTrackerAPI.Data;
    using CourseTrackerAPI.Models;
    using Microsoft.EntityFrameworkCore;

    public class CourseRepository : ICourseRepository
    {
        private readonly DataContext _context;
        public CourseRepository(DataContext context) => _context = context;

        public async Task<IEnumerable<Course>> GetAllCourses()
        {
            return await _context.Courses.ToListAsync();
        }

        public async Task<Course?> GetCourse(int id)
        {
            return await _context.Courses.FindAsync(id);
        }

        public async Task<IEnumerable<Course>> GetUserCourses(int userId)
        {
            return await _context.UserCourses
                .Where(uc => uc.UserId == userId)
                .Include(uc => uc.Course)
                .Select(uc => uc.Course)
                .ToListAsync();
        }

        public async Task<UserCourse> EnrollUserInCourse(int userId, int courseId)
        {
            var userCourse = new UserCourse { UserId = userId, CourseId = courseId };
            _context.UserCourses.Add(userCourse);
            await _context.SaveChangesAsync();
            return userCourse;
        }

        public async Task<bool> UnenrollUserFromCourse(int userId, int courseId)
        {
            var enrollment = await _context.UserCourses
                .FirstOrDefaultAsync(uc => uc.UserId == userId && uc.CourseId == courseId);

            if (enrollment == null) return false;

            _context.UserCourses.Remove(enrollment);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> IsUserEnrolledInCourse(int userId, int courseId)
        {
            return await _context.UserCourses
                .AnyAsync(uc => uc.UserId == userId && uc.CourseId == courseId);
        }

        public async Task<IEnumerable<Course>> SearchCourses(string searchTerm, int skip, int take)
        {
            return await _context.Courses
                .Where(c => c.Title.Contains(searchTerm) || c.Description.Contains(searchTerm))
                .Skip(skip)
                .Take(take)
                .ToListAsync();
        }

        public async Task<int> GetSearchResultsCount(string searchTerm)
        {
            return await _context.Courses
                .CountAsync(c => c.Title.Contains(searchTerm) || c.Description.Contains(searchTerm));
        }

        public async Task<int> GetCoursesCount()
        {
            return await _context.Courses.CountAsync();
        }

        public async Task<IEnumerable<Course>> GetCourse(int skip, int take)
        {
            return await _context.Courses
                .Skip(skip)
                .Take(take)
                .ToListAsync();
        }

        public async Task AddCourse(Course course)
        {
            _context.Courses.Add(course);
            await _context.SaveChangesAsync();
        }

       

        public Task<IEnumerable<Course>> GetCoursesForUser(int userId)
        {
            throw new NotImplementedException();
        }

        public async Task<int> GetEnrollmentCount(int courseId)
        {
            return await _context.UserCourses.CountAsync(uc => uc.CourseId == courseId);
        }


    }
}
