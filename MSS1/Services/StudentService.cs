using MSS1.DTOs.RequestDTOs;
using MSS1.DTOs.ResponseDTOs;
using MSS1.Entities;
using MSS1.Interfaces;
using MSS1.Repository;

namespace MSS1.Services
{
    public class StudentService: IStudentService
    {
        private readonly IStudentRepository _repository;

        public StudentService(IStudentRepository repository)
        {
            _repository = repository;
        }


        public async Task<List<StudentResponseDTO>> GetAllStudentsAsync()
        {
            var students = await _repository.GetAllStudentsAsync();
            return students.Select(s => new StudentResponseDTO
            {
                StudentId = s.StudentId,
                FullName = s.User.FullName,
                Email = s.User.Email,
                Address = s.User.Address,
                MobileNumber = s.User.MobileNumber,
                Gender = s.User.Gender,
                RegistrationFee = s.RegistrationFee,
                IsRegistrationFeePaid = s.IsRegistrationFeePaid,
                NICNumber = s.User.NICNumber,

                // Include NICNumber here
            }).ToList();
        }


        // Get Student By Id
        public async Task<StudentResponseDTO> GetStudentByIdAsync(int studentId)
        {
            var student = await _repository.GetStudentByIdAsync(studentId);
            if (student == null)
                throw new ArgumentException("Student not found.");

            return new StudentResponseDTO
            {
                //StudentId = student.StudentId,
                FullName = student.User.FullName,
                Email = student.User.Email,
                Address = student.User.Address,
                MobileNumber = student.User.MobileNumber,
                Gender = student.User.Gender,
                RegistrationFee = student.RegistrationFee,
                IsRegistrationFeePaid = student.IsRegistrationFeePaid
            };
        }

        // Delete Student By Id
        public async Task DeleteStudentAsync(int studentId)
        {
            await _repository.DeleteStudentAsync(studentId);
        }

        // Update Student
        public async Task<StudentResponseDTO> UpdateStudentAsync(int studentId, UpdateStudentRequestDTO request)
        {
            var updatedStudent = await _repository.UpdateStudentAsync(new Student
            {
                StudentId = studentId,
                User = new User
                {
                    FullName = request.FullName,
                    Email = request.Email,
                    Address = request.Address,
                    MobileNumber = request.MobileNumber,
                    Gender = request.Gender
                },
                RegistrationFee = request.RegistrationFee,
                IsRegistrationFeePaid = request.IsRegistrationFeePaid
            });

            return new StudentResponseDTO
            {
                //StudentId = updatedStudent.StudentId,
                FullName = updatedStudent.User.FullName,
                Email = updatedStudent.User.Email,
                Address = updatedStudent.User.Address,
                MobileNumber = updatedStudent.User.MobileNumber,
                Gender = updatedStudent.User.Gender,
                RegistrationFee = updatedStudent.RegistrationFee,
                IsRegistrationFeePaid = updatedStudent.IsRegistrationFeePaid
            };
        }




    }
}
