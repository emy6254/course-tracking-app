using System.ComponentModel.DataAnnotations;

namespace CourseTrackerAPI.DTOs.CourseDTO

{ 
    public class CourseDto
    {
        public int Id { get; set; }
        public required string Title { get; set; }
        public required string Description { get; set; }
        public string? Instructor { get; set; }
        public decimal Price { get; set; }
        public int Duration { get; set; }
        public string Level { get; set; } = "Beginner";
        public string? Category { get; set; }
        public string? ImageUrl { get; set; }
        public DateTime CreatedAt { get; set; }

        public int EnrolledCount { get; set; } = 0;
    }



public class CourseCreateDto
    {
        [Required]
        [StringLength(200, MinimumLength = 5)]
        public required string Title { get; set; }

        [Required]
        [StringLength(2000, MinimumLength = 20)]
        public required string Description { get; set; }

        [StringLength(100)]
        public string? Instructor { get; set; }

        [Range(0, 10000)]
        public decimal? Price { get; set; }

        [Range(1, 1000)]
        public int Duration { get; set; } = 1;

        [RegularExpression("^(Beginner|Intermediate|Advanced)$")]
        public string Level { get; set; } = "Beginner";

        [StringLength(50)]
        public string? Category { get; set; }

        [Url]
        public string? ImageUrl { get; set; }
    }

    public class CourseUpdateDto
    {
        [Required]
        [StringLength(200, MinimumLength = 5)]
        public required string Title { get; set; }

        [Required]
        [StringLength(2000, MinimumLength = 20)]
        public required string Description { get; set; }

        [StringLength(100)]
        public string? Instructor { get; set; }

        [Range(0, 10000)]
        public decimal? Price { get; set; }

        [Range(1, 1000)]
        public int Duration { get; set; }

        [RegularExpression("^(Beginner|Intermediate|Advanced)$")]
        public required string Level { get; set; }

        [StringLength(50)]
        public string? Category { get; set; }

        [Url]
        public string? ImageUrl { get; set; }

        public bool IsActive { get; set; } = true;

    }

    public class CourseSummaryDto
    {
        public int Id { get; set; }
        public required string Title { get; set; }
        public required string Description { get; set; }
        public string? Instructor { get; set; }
        public decimal? Price { get; set; }
        public int Duration { get; set; }
        public string? Level { get; set; }
        public string? Category { get; set; }
        public string? ImageUrl { get; set; }
        public int EnrolledUsersCount { get; set; }
        public double AverageRating { get; set; }
        public bool IsActive { get; set; }
    }
}
