namespace CourseTrackerAPI.Models
{
    public class Course
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string? Instructor { get; set; }
        public decimal Price { get; set; }
        public int Duration { get; set; }
        public string Level { get; set; } = "Beginner";
        public string? Category { get; set; }
        public string? ImageUrl { get; set; }
        public DateTime CreatedAt { get; set; }

        public ICollection<UserCourse> EnrolledUsers { get; set; } = new List<UserCourse>();
    }
}
