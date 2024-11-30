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

            // Create student-course entries
            var studentCourses = request.CourseIds.Select(courseId => new StudentCourse
            {
                StudentId = student.StudentId,  // Use the StudentId from the found student
                CourseId = courseId
            });

            // Add the courses for the student
            await _studentCourseRepository.AddStudentCoursesAsync(studentCourses);
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
                Description = c.Description
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
                    Description = c.Description
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
                    Description = sc.Course.Description
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
                Description = c.Description
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

    }
}
