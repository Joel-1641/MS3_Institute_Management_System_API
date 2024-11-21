﻿using MSS1.Entities;

namespace MSS1.Interfaces
{
    public interface ICourseRepository
    {
        Task<Course> AddCourseAsync(Course course);
        Task<Course> GetCourseByIdAsync(int courseId);
    }
}
