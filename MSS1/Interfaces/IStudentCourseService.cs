using Microsoft.AspNetCore.Mvc;
using MSS1.DTOs.RequestDTOs;
using MSS1.DTOs.ResponseDTOs;

namespace MSS1.Interfaces
{
    public interface IStudentCourseService
    {
        Task AddStudentCoursesAsync(AddStudentCourseRequestDTO request);
        Task<IEnumerable<CourseResponseDTO>> GetCoursesByStudentIdAsync(int studentId);
        Task<IEnumerable<StudentWithCourseResponseDTO>> GetAllStudentCoursesAsync();
        Task<StudentWithCourseResponseDTO> GetStudentWithCoursesByIdAsync(int studentId);
        Task<int> GetStudentCountForCourseAsync(int courseId);
        Task<string> GetStudentNICByIdAsync(int studentId);
        Task<IEnumerable<CourseResponseDTO>> GetAllCoursesAsync();
        Task<IEnumerable<CourseResponseDTO>> GetCoursesByNICAsync(string nic);
        Task<StudentTotalFeeResponseDTO> GetTotalFeeByNICAsync(string nic);
        //Task RecordPaymentAsync(string nic, decimal amount);
        Task<StudentPaymentStatusResponseDTO> GetPaymentStatusByNICAsync(string nic);
        Task<IEnumerable<StudentPaymentStatusResponseDTO>> GetAllStudentsPaymentStatusAsync();
        Task<CumulativePaymentStatusDTO> GetCumulativePaymentStatusAsync();
       Task<int> GetTotalCoursesByNICAsync(string nic);
       // Task AdminApprovePaymentAsync(int notificationId);
        Task<IEnumerable<NotificationResponseDTO>> GetAdminNotificationsAsync();
       // Task<IEnumerable<NotificationResponseDTO>> GetPaymentNotificationsByStudentIdAsync(int studentId);
        Task RecordPaymentAsync(string nic, decimal amount);
        Task<IEnumerable<NotificationResponseDTO>> GetNotificationsForStudentAsync(string nic);
     //   Task<IEnumerable<NotificationResponseDTO>> GetAdminNotificationsAsync();
        //Task<IActionResult> MarkNotificationAsRead(int notificationId);


    }
}

