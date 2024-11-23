using MSS1.DTOs.RequestDTOs;
using MSS1.DTOs.ResponseDTOs;
using MSS1.Entities;
using MSS1.Interfaces;

namespace MSS1.Services
{
    public class StudentService: IStudentService
    {
        private readonly IStudentRepository _repository;

        public StudentService(IStudentRepository repository)
        {
            _repository = repository;
        }

        public async Task<ProfileResposeDTO> GetStudentProfile(int studentId)
        {
            // Fetch the student with courses and payments
            var student = await _repository.GetStudentWithCoursesAndPaymentsAsync(studentId);
            if (student == null)
            {
                throw new KeyNotFoundException("Student not found.");
            }

            // Map the student, courses, and payments to the ProfileResponseDTO
            var profile = new ProfileResposeDTO
            {
                StudentId = student.StudentId,
                FullName = student.User.FullName,
                ProfilePicture = student.ProfilePicture,
                Courses = student.Courses.Select(c => new CourseDTO
                {
                    CourseId = c.CourseId,
                    CourseName = c.CourseName,
                    Level = c.Level,
                    CourseFee = c.CourseFee
                }).ToList(),
                Payments = student.Payments.Select(p => new PaymentDTO
                {
                    PaymentId = p.PaymentId,
                    AmountPaid = p.AmountPaid,
                    PaymentMethod = p.PaymentMethod,
                    PaymentDate = p.PaymentDate,
                    PaymentStatus = p.PaymentStatus
                }).ToList()
            };

            return profile;
        }

        public async Task UpdateStudentProfile(int studentId, UpdateStudentRequestDTO request)
        {
            var student = new Student
            {
                User = new User { FullName = request.FullName },
                ProfilePicture = request.ProfilePicture
            };
            await _repository.UpdateStudentProfile(studentId, student);
        }

        public async Task<List<Course>> GetStudentCourses(int studentId)
        {
            return await _repository.GetStudentCourses(studentId);
        }

        //public async Task EnrollInCourse(int studentId, int courseId)
        //{
        //    await _repository.EnrollInCourse(studentId, courseId);
        //}

        public async Task<List<Payment>> GetStudentPayments(int studentId)
        {
            return await _repository.GetStudentPayments(studentId);
        }

        public async Task AddPayment(AddPaymentRequestDTO request)
        {
            // Check if the student exists
            var student = await _repository.GetStudentById(request.StudentId);
            if (student == null)
            {
                throw new KeyNotFoundException("Student not found.");
            }

            // Create and save the payment
            var payment = new Payment
            {
                StudentId = request.StudentId,
                AmountPaid = request.Amount,
                PaymentMethod = request.PaymentMethod,
                PaymentDate = DateTime.UtcNow,
                IsForRegistrationFee = request.IsForRegistrationFee,
                PaymentStatus = "Completed"
            };

            await _repository.AddPayment(payment);
        }
        public async Task EnrollInCourse(int studentId, int courseId)
        {
            try
            {
                await _repository.EnrollInCourse(studentId, courseId);
            }
            catch (InvalidOperationException ex)
            {
                throw new ArgumentException(ex.Message);
            }
        }


    }
}
