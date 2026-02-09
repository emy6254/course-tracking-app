public class DashboardStatsDto
{
    public int TotalUsers { get; set; }
    public int TotalCourses { get; set; }
    public int TotalEnrollments { get; set; }
    public int ActiveUsers { get; set; }
    public int NewUsersThisMonth { get; set; }
    public int NewEnrollmentsThisMonth { get; set; }
    public double AverageCoursesPerUser { get; set; }
    public List<PopularCourseDto> PopularCourses { get; set; } = new List<PopularCourseDto>();
    public List<RecentActivityDto> RecentActivities { get; set; } = new List<RecentActivityDto>();
}

public class PopularCourseDto
{
    public int Id { get; set; }
    public required string Title { get; set; }
    public int EnrollmentCount { get; set; }
    public double AverageRating { get; set; }
}

public class RecentActivityDto
{
    public string Type { get; set; } = string.Empty; // Registration, Enrollment, Completion
    public string Description { get; set; } = string.Empty;
    public DateTime Timestamp { get; set; }
    public string UserName { get; set; } = string.Empty;
    public string? CourseName { get; set; }
}

public class UserStatsDto
{
    public int TotalEnrolledCourses { get; set; }
    public int CompletedCourses { get; set; }
    public int InProgressCourses { get; set; }
    public int DroppedCourses { get; set; }
    public double AverageProgress { get; set; }
    public double AverageRating { get; set; }
    public int TotalHoursLearned { get; set; }
    public DateTime? LastActivity { get; set; }
    public List<string> PreferredCategories { get; set; } = new List<string>();
}
