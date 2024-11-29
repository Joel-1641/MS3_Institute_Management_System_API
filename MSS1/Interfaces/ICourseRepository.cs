﻿using MSS1.Entities;

namespace MSS1.Interfaces
{
    public interface ICourseRepository
    {
        Task<IEnumerable<Course>> GetAllCoursesAsync();
        Task<Course> GetCourseByIdAsync(int courseId);
        Task<Course> AddCourseAsync(Course course);
        Task<Course> UpdateCourseAsync(Course course);
        Task<bool> DeleteCourseAsync(int courseId);
        Task<bool> IsDuplicateCourseAsync(string courseName, string level);
    }
}
