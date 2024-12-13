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

            // Send notification to Admin that the student is ready to pay
            var notification = new Notification
            {
                StudentId = student.StudentId,
                Message = $"Student {student.User.FullName} (NIC: {student.User.NICNumber}) has selected their courses and is now ready to pay.",
                IsRead = false,  // Admin has not read the notification
                CreatedAt = DateTime.Now,
                NotificationType = "Enrollment", // This is an enrollment notification, not payment
                Target = "Admin" // This notification is for the Admin
            };

            // Add the notification to the database for Admin
            await _studentCourseRepository.AddNotificationAsync(notification);
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
        public async Task RecordPaymentAndNotifyStudentAsync(string nic, decimal amount)
        {
            var student = await _studentCourseRepository.GetStudentByNICAsync(nic);

            if (student == null)
            {
                throw new KeyNotFoundException($"No student found with NIC {nic}.");
            }

            // Record the payment
            await _studentCourseRepository.AddPaymentAsync(new Payment
            {
                StudentId = student.StudentId,
                AmountPaid = amount,
                PaymentDate = DateTime.Now  // Ensure you save the payment date
            });

            // Check if the student's payment is fully processed
            var totalFee = await _studentCourseRepository.GetTotalFeeForStudentAsync(student.StudentId);
            var totalPaid = await _studentCourseRepository.GetTotalAmountPaidByStudentAsync(student.StudentId);

            // Determine if the student has fully paid or still owes money
            if (totalFee <= totalPaid)
            {
                // If fully paid, send a success message
                var notificationMessage = $"Your payment of {amount:C} has been successfully processed. You have fully paid for your course.";

                var notification = new Notification
                {
                    StudentId = student.StudentId,
                    Message = notificationMessage,
                    IsRead = false,  // New notification, hence unread
                    CreatedAt = DateTime.Now,
                    NotificationType = "Payment",  // Payment-related notification
                    Target = "Student"  // This notification is for the student
                };

                // Save the notification to the database
                await _studentCourseRepository.AddNotificationAsync(notification);
            }
            else
            {
                // If not fully paid, show how much is left to pay
                var remainingAmount = totalFee - totalPaid;
                var notificationMessage = $"Your payment of {amount:C} has been recorded. You still need to pay {remainingAmount:C} to complete your payment.";

                var notification = new Notification
                {
                    StudentId = student.StudentId,
                    Message = notificationMessage,
                    IsRead = false,  // New notification, hence unread
                    CreatedAt = DateTime.Now,
                    NotificationType = "Payment",  // Payment-related notification
                    Target = "Student"  // This notification is for the student
                };

                // Save the notification to the database
                await _studentCourseRepository.AddNotificationAsync(notification);
            }
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


        public async Task<CumulativePaymentStatusDTO> GetCumulativePaymentStatusAsync()
        {
            var students = await _studentCourseRepository.GetAllStudentsAsync(); // Get all students

            decimal cumulativeTotalFee = 0;
            decimal cumulativeTotalPaid = 0;
            decimal cumulativePaymentDue = 0;

            foreach (var student in students)
            {
                // Calculate total fee for each student
                var totalFee = await _studentCourseRepository.GetTotalFeeForStudentAsync(student.StudentId);

                // Calculate total paid for each student
                var totalPaid = await _studentCourseRepository.GetTotalAmountPaidByStudentAsync(student.StudentId);

                // Calculate payment due for each student
                var paymentDue = totalFee - totalPaid;

                // Add to cumulative totals
                cumulativeTotalFee += totalFee;
                cumulativeTotalPaid += totalPaid;
                cumulativePaymentDue += paymentDue;
            }

            // Return cumulative totals as a DTO
            return new CumulativePaymentStatusDTO
            {
                CumulativeTotalFee = cumulativeTotalFee,
                CumulativeTotalPaid = cumulativeTotalPaid,
                CumulativePaymentDue = cumulativePaymentDue
            };
        }

        public async Task<int> GetTotalCoursesByNICAsync(string nic)
        {
            // Get the student by NIC
            var student = await _studentCourseRepository.GetStudentByNICAsync(nic);

            if (student == null)
            {
                throw new KeyNotFoundException($"No student found with NIC {nic}.");
            }

            // Get the courses associated with the student
            var courses = await _studentCourseRepository.GetCoursesByStudentIdAsync(student.StudentId);

            // Return the count of courses
            return courses.Count();
        }

        // In your StudentCourseService class

        public async Task<IEnumerable<NotificationResponseDTO>> GetAdminNotificationsAsync()
        {
            // Retrieve all notifications, but we want only "Enrollment" type notifications for the Admin
            var notifications = await _studentCourseRepository.GetAdminNotificationsAsync();

            // Filter the notifications to only include those of type "Enrollment"
            var enrollmentNotifications = notifications
                .Where(n => n.NotificationType == "Enrollment" && n.Target == "Admin")
                .Select(n => new NotificationResponseDTO
                {
                    NotificationId = n.NotificationId,
                    Message = n.Message,
                    IsRead = n.IsRead,
                    CreatedAt = n.CreatedAt,
                    NotificationType = n.NotificationType,
                    Target = n.Target,
                    StudentName = n.Student?.User?.FullName,  // Assuming the Student has a User object with FullName property
                    StudentNIC = n.Student?.User?.NICNumber  // Assuming the Student has a User object with NICNumber property
                });

            // Return the filtered and mapped notifications
            return enrollmentNotifications;
        }

        //public async Task<IEnumerable<NotificationResponseDTO>> GetPaymentNotificationsByStudentIdAsync(int studentId)
        //{
        //    // Get all notifications for the student, filter by "Payment" type and unread status.
        //    var notifications = await _studentCourseRepository.GetNotificationsByStudentIdAsync(studentId);

        //    var paymentNotifications = notifications
        //        .Where(n => n.NotificationType == "Payment" && n.IsRead == false)
        //        .Select(n => new NotificationResponseDTO
        //        {
        //            NotificationId = n.NotificationId,
        //            Message = n.Message,
        //            IsRead = n.IsRead,
        //            CreatedAt = n.CreatedAt,
        //            NotificationType = n.NotificationType,
        //            Target = n.Target,
        //            StudentName = n.Student?.User.FullName,
        //            StudentNIC = n.Student?.User.NICNumber
        //        });

        //    return paymentNotifications;
        //}

        public async Task RecordPaymentAsync(string nic, decimal amount)
        {
            // Retrieve the student by NIC
            var student = await _studentCourseRepository.GetStudentByNICAsync(nic);

            if (student == null)
            {
                throw new KeyNotFoundException($"No student found with NIC {nic}.");
            }

            // Record the payment (even if it's partial or the student is ready to pay)
            await _studentCourseRepository.AddPaymentAsync(new Payment
            {
                StudentId = student.StudentId,
                AmountPaid = amount,
                PaymentDate = DateTime.Now  // Ensure you save the payment date
            });

            // Check if the student's payment is fully processed
            var totalFee = await _studentCourseRepository.GetTotalFeeForStudentAsync(student.StudentId);
            var totalPaid = await _studentCourseRepository.GetTotalAmountPaidByStudentAsync(student.StudentId);

            // Determine if the student has fully paid or still owes money
            if (totalFee <= totalPaid)
            {
                // If fully paid, send a success message
                var notificationMessage = $"Your payment of {amount:C} has been successfully processed. You have fully paid for your course.";

                var notification = new Notification
                {
                    StudentId = student.StudentId,
                    Message = notificationMessage,
                    IsRead = false,  // New notification, hence unread
                    CreatedAt = DateTime.Now,
                    NotificationType = "Payment",  // Payment-related notification
                    Target = "Student"  // This notification is for the student
                };

                // Save the notification to the database
                await _studentCourseRepository.AddNotificationAsync(notification);
            }
            else
            {
                // If not fully paid, show how much is left to pay
                var remainingAmount = totalFee - totalPaid;
                var notificationMessage = $"Your payment of {amount:C} has been recorded. You still need to pay {remainingAmount:C} to complete your payment.";

                var notification = new Notification
                {
                    StudentId = student.StudentId,
                    Message = notificationMessage,
                    IsRead = false,  // New notification, hence unread
                    CreatedAt = DateTime.Now,
                    NotificationType = "Payment",  // Payment-related notification
                    Target = "Student"  // This notification is for the student
                };

                // Save the notification to the database
                await _studentCourseRepository.AddNotificationAsync(notification);
            }
        }

        public async Task<IEnumerable<NotificationResponseDTO>> GetNotificationsForStudentAsync(string nic)
        {
            // Step 1: Retrieve the student by NIC
            var student = await _studentCourseRepository.GetStudentByNICAsync(nic);

            if (student == null)
            {
                throw new KeyNotFoundException($"Student with NIC {nic} not found.");
            }

            // Step 2: Retrieve notifications for the student
            var notifications = await _studentCourseRepository.GetNotificationsForStudentAsync(student.StudentId);

            // Step 3: Map to NotificationResponseDTO
            var notificationDtos = notifications.Select(n => new NotificationResponseDTO
            {
                NotificationId = n.NotificationId,
                Message = n.Message,
                IsRead = n.IsRead,
                CreatedAt = n.CreatedAt,
                NotificationType = n.NotificationType,
                Target = n.Target,
                StudentName = n.Student?.User?.FullName, // Assuming you have the FullName field in the Student's User entity
                StudentNIC = n.Student?.User?.NICNumber  // Assuming you have the NICNumber field in the Student's User entity
            });

            // Step 4: Return the mapped notifications
            return notificationDtos;
        }






    }
}
