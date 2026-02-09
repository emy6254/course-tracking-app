using System.ComponentModel.DataAnnotations;
using System;
using CourseTrackerAPI.DTOs.CourseDTO;
using System.Text.Json.Serialization;
using System.Text.RegularExpressions;
using System.Linq;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;




namespace CourseTrackerAPI.DTOs
{
    public class EnrollmentDto
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int CourseId { get; set; }
        public DateTime EnrollmentDate { get; set; }
        public string Status { get; set; } = "Active"; // Active, Completed, Dropped
        public double? Progress { get; set; } // 0-100
        public DateTime? CompletionDate { get; set; }
        public int? Rating { get; set; } // 1-5 stars
        public string? Review { get; set; }
        public required UserSummaryDto User { get; set; }
        public required CourseSummaryDto Course { get; set; }
    }

    public class EnrollmentCreateDto
    {
        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "Valid course ID is required")]
        public int CourseId { get; set; }
    }

    public class EnrollmentUpdateDto
    {
        [RegularExpression("^(Active|Completed|Dropped)$", ErrorMessage = "Status must be Active, Completed, or Dropped")]
        public string Status { get; set; } = "Active";

        [Range(0, 100, ErrorMessage = "Progress must be between 0 and 100")]
        public double? Progress { get; set; }

        [Range(1, 5, ErrorMessage = "Rating must be between 1 and 5")]
        public int? Rating { get; set; }

        [StringLength(1000, ErrorMessage = "Review cannot exceed 1000 characters")]
        public string? Review { get; set; }
    }

}
