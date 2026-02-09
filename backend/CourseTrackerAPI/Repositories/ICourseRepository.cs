using CourseTrackerAPI.Models;

namespace CourseTrackerAPI.Repositories
{
    public interface ICourseRepository
    {
        Task<IEnumerable<Course>> GetAllCourses();
        Task<Course?> GetCourse(int id);
        Task<IEnumerable<Course>> GetUserCourses(int userId);
        Task<UserCourse> EnrollUserInCourse(int userId, int courseId);
        Task<bool> UnenrollUserFromCourse(int userId, int courseId);
        Task<bool> IsUserEnrolledInCourse(int userId, int courseId);
        Task<IEnumerable<Course>> SearchCourses(string searchTerm, int skip, int take);
        Task<int> GetSearchResultsCount(string searchTerm);
        Task<int> GetCoursesCount();
        Task<IEnumerable<Course>> GetCourse(int skip, int take);
        Task AddCourse(Course course);

        Task<int> GetEnrollmentCount(int courseId);
    }
}
