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
       // Task<int> GetStudentCountForCourseAsync(int courseId);
        Task<IEnumerable<Course>> GetAllCoursesAsync();
        Task<IEnumerable<StudentCourse>> GetStudentCoursesByStudentIdAsync(int studentId);
        Task<Student> GetStudentByNICAsync(string nic);
        Task<int?> GetCourseIdByNameAndLevelAsync(string courseName, string level);
        Task<bool> IsStudentEnrolledInCourseAsync(int studentId, int courseId);
        //Task<string> GetStudentNICByIdAsync(int studentId);
        Task<decimal> GetTotalFeeForStudentAsync(int studentId);
        Task AddPaymentAsync(Payment payment);
        Task<decimal> GetTotalAmountPaidByStudentAsync(int studentId);
        //Task<List<StudentPaymentSummaryDTO>> GetAllStudentsPaymentStatusAsync();
        Task<IEnumerable<Student>> GetAllStudentsAsync();
        Task UpdateNotificationAsync(Notification notification);
        Task<Notification> GetNotificationByIdAsync(int notificationId);
        Task AddNotificationAsync(Notification notification);
        Task<IEnumerable<Notification>> GetAdminNotificationsAsync();
        Task<IEnumerable<Notification>> GetNotificationsByStudentIdAsync(int studentId);
        Task<IEnumerable<Notification>> GetNotificationsForStudentAsync(int studentId);
        Task<List<CourseStudentCountDTO>> GetAllCoursesWithStudentCountAsync();
       // Task<List<CourseStudentCountDTO>> GetAllCoursesWithStudentCountAsync()
        //Task<Student> GetStudentByNICAsync(string nic);
        //   Task<Student> GetStudentByNICAsync(string nic);



        }
}
