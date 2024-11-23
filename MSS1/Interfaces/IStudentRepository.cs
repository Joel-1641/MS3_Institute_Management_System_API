﻿using MSS1.Entities;

namespace MSS1.Interfaces
{
    public interface IStudentRepository
    {
        Task<Student> GetStudentProfile(int studentId);
        Task UpdateStudentProfile(int studentId, Student student);
        Task<List<Course>> GetStudentCourses(int studentId);
        //Task EnrollInCourse(int studentId, int courseId);
        Task<List<Payment>> GetStudentPayments(int studentId);
        Task AddPayment(Payment payment);
        Task<Student> GetStudentById(int studentId);
        Task AddStudentAsync(Student student);
        Task EnrollInCourse(int studentId, int courseId);
        // Task<Student> GetStudentWithCoursesAsync(int studentId);
        Task<Student> GetStudentWithCoursesAndPaymentsAsync(int studentId);
    }
}
