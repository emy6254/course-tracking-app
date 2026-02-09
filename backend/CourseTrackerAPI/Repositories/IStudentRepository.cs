namespace CourseTrackerAPI.Repositories
{
    using CourseTrackerAPI.Models;

    public interface IStudentRepository
    {
        Task<IEnumerable<Student>> GetAll();
        Task<Student?> GetById(int id);
        Task<Student> Update(Student student);
        Task<bool> Delete(int id);
    }
}
