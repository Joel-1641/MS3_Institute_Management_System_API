﻿using MSS1.DTOs.ResponseDTOs;
using MSS1.Entities;

namespace MSS1.Interfaces
{
    public interface IStudentCourseRepository
    {
        Task AddStudentCoursesAsync(IEnumerable<StudentCourse> studentCourses);
        Task<Student> GetStudentByIdAsync(int studentId);
        Task<string> GetStudentNICByIdAsync(int studentId);
        Task<IEnumerable<Course>> GetCoursesByStudentIdAsync(int studentId);
        Task<IEnumerable<(Student Student, IEnumerable<Course> Courses)>> GetAllStudentCoursesAsync();
        Task<int> GetStudentCountForCourseAsync(int courseId);
        Task<IEnumerable<Course>> GetAllCoursesAsync();
        Task<IEnumerable<StudentCourse>> GetStudentCoursesByStudentIdAsync(int studentId);
        Task<Student> GetStudentByNICAsync(string nic);
        Task<int?> GetCourseIdByNameAndLevelAsync(string courseName, string level);
        Task<bool> IsStudentEnrolledInCourseAsync(int studentId, int courseId);
        //Task<string> GetStudentNICByIdAsync(int studentId);
        Task<decimal> GetTotalFeeForStudentAsync(int studentId);
        Task AddPaymentAsync(Payment payment);
        Task<decimal> GetTotalAmountPaidByStudentAsync(int studentId);


    }
}
