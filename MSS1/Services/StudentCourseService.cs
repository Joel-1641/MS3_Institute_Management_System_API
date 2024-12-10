using MSS1.DTOs.RequestDTOs;
using MSS1.DTOs.ResponseDTOs;
using MSS1.Entities;
using MSS1.Interfaces;

namespace MSS1.Services
{
    public class StudentCourseService : IStudentCourseService
    {
        private readonly IStudentCourseRepository _studentCourseRepository;

        public StudentCourseService(IStudentCourseRepository studentCourseRepository)
        {
            _studentCourseRepository = studentCourseRepository;
        }

        public async Task AddStudentCoursesAsync(AddStudentCourseRequestDTO request)
        {
            // Find the student by NIC
            var student = await _studentCourseRepository.GetStudentByNICAsync(request.NIC);

            if (student == null)
            {
                throw new KeyNotFoundException($"No student found with NIC {request.NIC}.");
            }

            var errorMessages = new List<string>();
            var studentCourses = new List<StudentCourse>();

            foreach (var course in request.Courses)
            {
                // Get the CourseId by CourseName and Level
                var courseId = await _studentCourseRepository.GetCourseIdByNameAndLevelAsync(course.CourseName, course.Level);

                if (courseId == null)
                {
                    errorMessages.Add($"No course found with Name: {course.CourseName} and Level: {course.Level}.");
                    continue;
                }

                // Check if the student is already enrolled in the course
                var alreadyEnrolled = await _studentCourseRepository.IsStudentEnrolledInCourseAsync(student.StudentId, courseId.Value);

                if (alreadyEnrolled)
                {
                    errorMessages.Add($"Student is already enrolled in Course: {course.CourseName} with Level: {course.Level}.");
                    continue;
                }

                // Add to the list if not enrolled
                studentCourses.Add(new StudentCourse
                {
                    StudentId = student.StudentId,
                    CourseId = courseId.Value,
                   // EnrollDate = DateOnly.FromDateTime(DateTime.Now)
                });
            }

            // Save new courses
            if (studentCourses.Any())
            {
                await _studentCourseRepository.AddStudentCoursesAsync(studentCourses);
            }

            // Handle errors
            if (errorMessages.Any())
            {
                throw new InvalidOperationException(string.Join("; ", errorMessages));
            }
        }

        public async Task<IEnumerable<CourseResponseDTO>> GetCoursesByStudentIdAsync(int studentId)
        {
            var courses = await _studentCourseRepository.GetCoursesByStudentIdAsync(studentId);
            return courses.Select(c => new CourseResponseDTO
            {
                CourseId = c.CourseId,
                CourseName = c.CourseName,
                Level = c.Level,
                CourseFee = c.CourseFee,
                Description = c.Description,
                CourseDuration =c.CourseDuration,
                CourseType = c.CourseType,
               // EnrollDate = c.EnrollDate,
               // CourseStartDate =c.CourseStartDate,
                //CourseEndDate =c.CourseEndDate
            });
        }

        public async Task<IEnumerable<StudentWithCourseResponseDTO>> GetAllStudentCoursesAsync()
        {
            var studentCourses = await _studentCourseRepository.GetAllStudentCoursesAsync();
            return studentCourses.Select(sc => new StudentWithCourseResponseDTO
            {
                StudentId = sc.Student.StudentId,
                StudentName = sc.Student.User.FullName,
                Courses = sc.Courses.Select(c => new CourseResponseDTO
                {
                    CourseId = c.CourseId,
                    CourseName = c.CourseName,
                    Level = c.Level,
                    CourseFee = c.CourseFee,
                    Description = c.Description,
                    CourseDuration = c.CourseDuration,
                    CourseType = c.CourseType,
                    
                    
                    
                   // CourseStartDate =c.CourseStartDate,
                    //CourseEndDate = c.CourseEndDate,
                }).ToList()
            });
        }

        public async Task<StudentWithCourseResponseDTO> GetStudentWithCoursesByIdAsync(int studentId)
        {
            // Retrieve the student's courses from the repository
            var studentCourses = await _studentCourseRepository.GetStudentCoursesByStudentIdAsync(studentId);

            // If no courses are found for the student, throw an exception
            var student = studentCourses.FirstOrDefault()?.Student;

            if (student == null)
            {
                throw new KeyNotFoundException($"No student found with ID {studentId}.");
            }

            // Construct the response DTO
            return new StudentWithCourseResponseDTO
            {
                StudentId = student.StudentId,
                StudentName = student.User.FullName,
                Courses = studentCourses.Select(sc => new CourseResponseDTO
                {
                    CourseId = sc.Course.CourseId,
                    CourseName = sc.Course.CourseName,
                    Level = sc.Course.Level,
                    CourseFee = sc.Course.CourseFee,
                    Description = sc.Course.Description,
                    CourseDuration = sc.Course.CourseDuration,
                    CourseType = sc.Course.CourseType,
                    EnrollDate = sc.EnrollDate,
                  //  CourseStartDate =sc.Course.CourseStartDate,
                    //CourseEndDate =sc.Course.CourseEndDate
                }).ToList()
            };
        }


        public async Task<int> GetStudentCountForCourseAsync(int courseId)
        {
            return await _studentCourseRepository.GetStudentCountForCourseAsync(courseId);
        }

        public async Task<IEnumerable<CourseResponseDTO>> GetAllCoursesAsync()
        {
            var courses = await _studentCourseRepository.GetAllCoursesAsync();
            return courses.Select(c => new CourseResponseDTO
            {
                CourseId = c.CourseId,
                CourseName = c.CourseName,
                Description = c.Description,
                Level = c.Level,
                CourseFee = c.CourseFee,
                CourseDuration = c.CourseDuration,
                CourseType = c.CourseType,
              //  CourseStartDate =c.CourseStartDate,
                //CourseEndDate =c.CourseEndDate
            });
        }

        public async Task<string> GetStudentNICByIdAsync(int studentId)
        {
            return await _studentCourseRepository.GetStudentNICByIdAsync(studentId);
        }
        // StudentCourseService.cs

        public async Task<Student> GetStudentByNICAsync(string nic)
        {
            var student = await _studentCourseRepository.GetStudentByNICAsync(nic);
            if (student == null)
            {
                throw new KeyNotFoundException($"No student found with NIC {nic}.");
            }
            return student;
        }
        public async Task<IEnumerable<CourseResponseDTO>> GetCoursesByNICAsync(string nic)
        {
            // Get the student using the NIC
            var student = await _studentCourseRepository.GetStudentByNICAsync(nic);
            if (student == null)
            {
                throw new KeyNotFoundException($"No student found with NIC {nic}.");
            }

            // Get the courses associated with the student
            var courses = await _studentCourseRepository.GetCoursesByStudentIdAsync(student.StudentId);

            // Map to DTO
            return courses.Select(c => new CourseResponseDTO
            {
                CourseId = c.CourseId,
                CourseName = c.CourseName,
                Level = c.Level,
                CourseFee = c.CourseFee,
                Description = c.Description,
                CourseDuration = c.CourseDuration,
                CourseType = c.CourseType,
                //CourseStartDate =c.CourseStartDate,
                //CourseEndDate = c.CourseEndDate
            });
        }
        public async Task<StudentTotalFeeResponseDTO> GetTotalFeeByNICAsync(string nic)
        {
            var student = await _studentCourseRepository.GetStudentByNICAsync(nic);

            if (student == null)
            {
                throw new KeyNotFoundException($"No student found with NIC {nic}.");
            }

            var totalFee = await _studentCourseRepository.GetTotalFeeForStudentAsync(student.StudentId);

            return new StudentTotalFeeResponseDTO
            {
                StudentId = student.StudentId,
                StudentName = student.User.FullName,
                TotalFee = totalFee
            };
        }
        public async Task RecordPaymentAsync(string nic, decimal amount)
        {
            var student = await _studentCourseRepository.GetStudentByNICAsync(nic);

            if (student == null)
            {
                throw new KeyNotFoundException($"No student found with NIC {nic}.");
            }

            // Add the payment
            await _studentCourseRepository.AddPaymentAsync(new Payment
            {
                StudentId = student.StudentId,
                AmountPaid = amount
            });
        }
        public async Task<StudentPaymentStatusResponseDTO> GetPaymentStatusByNICAsync(string nic)
{
    // Retrieve the student by NIC
    var student = await _studentCourseRepository.GetStudentByNICAsync(nic);

    if (student == null)
    {
        throw new KeyNotFoundException($"No student found with NIC {nic}.");
    }

    // Calculate total fee
    var totalFee = await _studentCourseRepository.GetTotalFeeForStudentAsync(student.StudentId);

    // Calculate total amount paid
    var totalPaid = await _studentCourseRepository.GetTotalAmountPaidByStudentAsync(student.StudentId);

    // Calculate payment due
    var paymentDue = totalFee - totalPaid;

    // Determine payment status
    var paymentStatus = paymentDue > 0 ? "Pending" : "Success";

    // Return the response DTO
    return new StudentPaymentStatusResponseDTO
    {
        StudentId = student.StudentId,
        StudentName = student.User.FullName,
        TotalFee = totalFee,
        TotalPaid = totalPaid,
        PaymentDue = paymentDue,
        PaymentStatus = paymentStatus
    };
}
        public async Task<IEnumerable<StudentPaymentStatusResponseDTO>> GetAllStudentsPaymentStatusAsync()
        {
            var students = await _studentCourseRepository.GetAllStudentsAsync(); // Call the method from repo

            var studentPaymentStatuses = new List<StudentPaymentStatusResponseDTO>();

            foreach (var student in students)
            {
                // Calculate total fee
                var totalFee = await _studentCourseRepository.GetTotalFeeForStudentAsync(student.StudentId);

                // Calculate total amount paid
                var totalPaid = await _studentCourseRepository.GetTotalAmountPaidByStudentAsync(student.StudentId);

                // Calculate payment due
                var paymentDue = totalFee - totalPaid;

                // Determine payment status
                var paymentStatus = paymentDue > 0 ? "Pending" : "Success";

                studentPaymentStatuses.Add(new StudentPaymentStatusResponseDTO
                {
                    StudentId = student.StudentId,
                    StudentName = student.User.FullName,
                    TotalFee = totalFee,
                    TotalPaid = totalPaid,
                    PaymentDue = paymentDue,
                    PaymentStatus = paymentStatus,
                    NIC = student.User.NICNumber // Include NIC here
                });
            }

            return studentPaymentStatuses;
        }






    }
}
