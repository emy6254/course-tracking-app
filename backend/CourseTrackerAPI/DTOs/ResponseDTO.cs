using System.ComponentModel.DataAnnotations;
using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;
using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;



namespace CourseTrackerAPI.DTOs
{
    public class ApiResponse<T>
    {
        public bool Success { get; set; }
        public string Message { get; set; } = string.Empty;
        public T? Data { get; set; }
        public List<string> Errors { get; set; } = new List<string>();
        public DateTime Timestamp { get; set; } = DateTime.UtcNow;
    }

    public class PagedResponse<T>
    {
        public bool Success { get; set; } = true;
        public string Message { get; set; } = string.Empty;
        public List<T> Data { get; set; } = new List<T>();
        public PaginationDto Pagination { get; set; } = new PaginationDto();
        public DateTime Timestamp { get; set; } = DateTime.UtcNow;
    }

    public class PaginationDto
    {
        public int CurrentPage { get; set; }
        public int PageSize { get; set; }
        public int TotalCount { get; set; }
        public int TotalPages { get; set; }
        public bool HasPrevious => CurrentPage > 1;
        public bool HasNext => CurrentPage < TotalPages;
    }

    public class AuthResponseDto
    {
        public required string Token { get; set; }
        public DateTime ExpiresAt { get; set; }
        public required UserDto User { get; set; }
        public string TokenType { get; set; } = "Bearer";
    }

    public class RefreshTokenDto
    {
        [Required(ErrorMessage = "Refresh token is required")]
        public required string RefreshToken { get; set; }
    }

}
