﻿namespace MSS1.Entities
{
    public class StudentCourse
    {
        public int StudentId { get; set; }
        public int CourseId { get; set; }
        public DateTime EnrollDate { get; set; } = DateTime.Now;
        public string CourseImg { get; set; }
       
        // Navigation properties
        public Student Student { get; set; }
        public Course Course { get; set; }

    }
}
