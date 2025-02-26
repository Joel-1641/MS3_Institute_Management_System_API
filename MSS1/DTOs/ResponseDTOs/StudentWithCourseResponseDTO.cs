﻿namespace MSS1.DTOs.ResponseDTOs
{
    public class StudentWithCourseResponseDTO
    {
        public int StudentId { get; set; }
        public string StudentName { get; set; }
        public DateTime EnrollDate { get; set; } 
        public IEnumerable<CourseResponseDTO> Courses { get; set; }
        public string CourseImg { get; set; }
    }
}
