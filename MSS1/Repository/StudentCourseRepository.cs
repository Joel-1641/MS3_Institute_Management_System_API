using Microsoft.EntityFrameworkCore;
using MSS1.Database;
using MSS1.DTOs.ResponseDTOs;
using MSS1.Entities;
using MSS1.Interfaces;

namespace MSS1.Repository
{
    public class StudentCourseRepository : IStudentCourseRepository
    {
        private readonly ITDbContext _context;

        public StudentCourseRepository(ITDbContext context)
        {
            _context = context;
        }

        public async Task AddStudentCoursesAsync(IEnumerable<StudentCourse> studentCourses)
        {
            foreach (var studentCourse in studentCourses)
            {
                var alreadyEnrolled = await _context.StudentCourses
                    .AnyAsync(sc => sc.StudentId == studentCourse.StudentId && sc.CourseId == studentCourse.CourseId);

                if (!alreadyEnrolled)
                {
                    await _context.StudentCourses.AddAsync(studentCourse);
                }
            }
            await _context.SaveChangesAsync();
        }

        public async Task<Student> GetStudentByIdAsync(int studentId)
        {
            return await _context.Students
                .Include(s => s.User)
                .FirstOrDefaultAsync(s => s.StudentId == studentId);
        }

        public async Task<string> GetStudentNICByIdAsync(int studentId)
        {
            var student = await _context.Students
                .Include(s => s.User)
                .FirstOrDefaultAsync(s => s.StudentId == studentId);

            if (student == null)
            {
                throw new KeyNotFoundException($"No student found with ID {studentId}.");
            }

            return student.User.NICNumber;
        }

        public async Task<IEnumerable<Course>> GetCoursesByStudentIdAsync(int studentId)
        {
            return await _context.StudentCourses
                .Where(sc => sc.StudentId == studentId)
                .Select(sc => sc.Course)
                .ToListAsync();
        }
      
        public async Task<IEnumerable<(Student Student, IEnumerable<Course> Courses)>> GetAllStudentCoursesAsync()
        {
            var studentCourses = await _context.StudentCourses
                .Include(sc => sc.Student)
                .ThenInclude(s => s.User)
                .Include(sc => sc.Course)
                .ToListAsync();

            return studentCourses
                .GroupBy(sc => sc.Student)
                .Select(group => (group.Key, group.Select(sc => sc.Course)));
        }

        public async Task<List<CourseStudentCountDTO>> GetAllCoursesWithStudentCountAsync()
        {
            return await _context.Courses
                .Select(course => new CourseStudentCountDTO
                {
                    CourseId = course.CourseId,
                    CourseName = course.CourseName,
                    Level = course.Level,
                    StudentCount = course.StudentCourses.Count()
                })
                .ToListAsync();
        }


        public async Task<IEnumerable<Course>> GetAllCoursesAsync()
        {
            return await _context.Courses.ToListAsync();
        }
        public async Task<Student> GetStudentByNICAsync(string nic)
        {
            return await _context.Students
                .Include(s => s.User)  // Assuming the NIC is stored in the User entity
                .FirstOrDefaultAsync(s => s.User.NICNumber == nic);  // Modify according to your actual NIC field
        }
        public async Task<IEnumerable<StudentCourse>> GetStudentCoursesByStudentIdAsync(int studentId)
        {
            return await _context.StudentCourses
                .Include(sc => sc.Student) // Include Student details
                .Include(sc => sc.Course)  // Include Course details
                .Where(sc => sc.StudentId == studentId)
                .ToListAsync();
        }

        public async Task<int?> GetCourseIdByNameAndLevelAsync(string courseName, string level)
        {
            var course = await _context.Courses
                .FirstOrDefaultAsync(c => c.CourseName == courseName && c.Level == level);
            return course?.CourseId;
        }
        public async Task<bool> IsStudentEnrolledInCourseAsync(int studentId, int courseId)
        {
            return await _context.StudentCourses
                .AnyAsync(sc => sc.StudentId == studentId && sc.CourseId == courseId);
        }
        public async Task<decimal> GetTotalFeeForStudentAsync(int studentId)
        {
            // Calculate the total fee by summing the CourseFee of the student's enrolled courses
            return await _context.StudentCourses
                .Where(sc => sc.StudentId == studentId)
                .SumAsync(sc => sc.Course.CourseFee);
        }
        public async Task AddPaymentAsync(Payment payment)
        {
            await _context.Payments.AddAsync(payment);
            await _context.SaveChangesAsync();
        }
        public async Task<decimal> GetTotalAmountPaidByStudentAsync(int studentId)
        {
            return await _context.Payments
                .Where(p => p.StudentId == studentId)
                .SumAsync(p => p.AmountPaid);
        }
        // Repository Method
        public async Task<IEnumerable<Student>> GetAllStudentsAsync()
        {
            return await _context.Students
                .Include(s => s.User) // Include User to access student details like name, NIC, etc.
                .ToListAsync();
        }
        public async Task AddNotificationAsync(Notification notification)
        {
            // Add the notification to the database
            await _context.Notifications.AddAsync(notification);
            await _context.SaveChangesAsync();
        }

        // 2. Get all unread notifications for admin
        public async Task<IEnumerable<Notification>> GetAdminNotificationsAsync()
        {
            // Fetch all notifications where the target is "Admin" and is unread
            return await _context.Notifications
                .Where(n => n.Target == "Admin" && !n.IsRead)
                .OrderByDescending(n => n.CreatedAt)  // Sort by date (most recent first)
                .ToListAsync();
        }

        // 3. Get a specific notification by ID
        public async Task<Notification> GetNotificationByIdAsync(int notificationId)
        {
            // Find the notification by its ID
            return await _context.Notifications
                .FirstOrDefaultAsync(n => n.NotificationId == notificationId);
        }

        // 4. Update a notification (mark it as read, or update message, etc.)
        public async Task UpdateNotificationAsync(Notification notification)
        {
            // First, find the existing notification
            var existingNotification = await _context.Notifications
                .FirstOrDefaultAsync(n => n.NotificationId == notification.NotificationId);

            if (existingNotification != null)
            {
                // Update the necessary fields
                existingNotification.IsRead = notification.IsRead;
                existingNotification.Message = notification.Message;
                existingNotification.CreatedAt = notification.CreatedAt;

                // Save changes to the database
                await _context.SaveChangesAsync();
            }
            else
            {
                throw new KeyNotFoundException("Notification not found.");
            }
        }

        public async Task<IEnumerable<Notification>> GetNotificationsByStudentIdAsync(int studentId)
        {
            return await _context.Notifications
                .Where(n => n.StudentId == studentId)
                .OrderByDescending(n => n.CreatedAt)  // Sorting by the most recent notifications
                .ToListAsync();
        }

        public async Task<IEnumerable<Notification>> GetNotificationsForStudentAsync(int studentId)
        {
            // Assuming you're using Entity Framework
            return await _context.Notifications
                                    .Where(n => n.StudentId == studentId)
                                    .OrderByDescending(n => n.CreatedAt) // Sort notifications by the most recent
                                    .ToListAsync();
        }

      








    }
}
