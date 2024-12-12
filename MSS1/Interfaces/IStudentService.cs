using MSS1.DTOs.RequestDTOs;
using MSS1.DTOs.ResponseDTOs;
using MSS1.Entities;

namespace MSS1.Interfaces
{
    public interface IStudentService
    {
        Task<List<StudentResponseDTO>> GetAllStudentsAsync();
        Task<StudentResponseDTO> GetStudentByIdAsync(int studentId);
        Task DeleteStudentAsync(int studentId);
        Task<StudentResponseDTO> UpdateStudentAsync(int studentId, UpdateStudentRequestDTO request);
        Task<StudentResponseDTO> GetStudentByNICNumberAsync(string nicNumber);
        Task<int> GetTotalStudentCountAsync();
       // Task<Student> GetStudentProfile(int studentId);
       // Task UpdateStudentProfile(int studentId, UpdateStudentRequestDTO request);
       // Task<List<Course>> GetStudentCourses(int studentId);
       //// Task EnrollInCourse(int studentId, int courseId);
       // Task<List<Payment>> GetStudentPayments(int studentId);
       // Task AddPayment(AddPaymentRequestDTO request);
       // Task EnrollInCourse(int studentId, int courseId);
       // Task<ProfileResposeDTO> GetStudentProfile(int studentId);
    }
}
