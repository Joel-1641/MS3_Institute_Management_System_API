using MSS1.DTOs.ResponseDTOs;
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
        //Task<string> GetStudentNICByIdAsync(int studentId);
    }
}
