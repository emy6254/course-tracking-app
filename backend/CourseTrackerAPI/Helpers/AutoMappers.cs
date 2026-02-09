using AutoMapper;
using CourseTrackerAPI.DTOs;
using CourseTrackerAPI.DTOs.CourseDTO;
using CourseTrackerAPI.Models;

public class AutoMapperProfiles : Profile
{
    public AutoMapperProfiles()
    {
        // User mappings  
        CreateMap<UserRegisterDto, User>();

        CreateMap<User, UserDto>()
            .ForMember(dest => dest.EnrolledCourses, opt =>
                opt.MapFrom(src => src.EnrolledCourses.Select(uc => uc.Course)));

        // Course mappings  
        CreateMap<Course, CourseDto>()
            .ForMember(dest => dest.EnrolledCount, opt => opt.Ignore());



        CreateMap<CourseCreateDto, Course>()
            .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => DateTime.UtcNow))
            .ForMember(dest => dest.Id, opt => opt.Ignore());

        CreateMap<CourseUpdateDto, Course>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.CreatedAt, opt => opt.Ignore());

        CreateMap<User, UserDto>()
    .ForMember(dest => dest.EnrolledCourses,
        opt => opt.MapFrom(src => src.EnrolledCourses.Select(uc => uc.Course)));

    }
}
