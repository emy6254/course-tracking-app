using System.ComponentModel.DataAnnotations;
using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;
using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.Linq;


namespace CourseTrackerAPI.DTOs
{
    public class CourseSearchDto
    {
        public string? SearchTerm { get; set; }
        public string? Category { get; set; }
        public string? Level { get; set; }
        public decimal? MinPrice { get; set; }
        public decimal? MaxPrice { get; set; }
        public int? MinDuration { get; set; }
        public int? MaxDuration { get; set; }
        public string? Instructor { get; set; }
        public bool? IsActive { get; set; } = true;
        public string SortBy { get; set; } = "Title"; // Title, Price, Duration, Rating, CreatedAt
        public string SortDirection { get; set; } = "ASC"; // ASC, DESC
        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 10;
    }

    public class UserSearchDto
    {
        public string? SearchTerm { get; set; }
        public string? Role { get; set; }
        public bool? IsActive { get; set; }
        public DateTime? CreatedAfter { get; set; }
        public DateTime? CreatedBefore { get; set; }
        public string SortBy { get; set; } = "Username"; // Username, Email, CreatedAt, LastLoginAt
        public string SortDirection { get; set; } = "ASC"; // ASC, DESC
        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 10;
    }

}
