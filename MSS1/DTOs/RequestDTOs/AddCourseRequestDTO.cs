﻿using System.Security.Permissions;

namespace MSS1.DTOs.RequestDTOs
{
    public class AddCourseRequestDTO
    {
      
        
        public string CourseName { get; set; }

       
        public string Level { get; set; } // e.g., Beginner, Intermediate, Advanced

       
        public decimal CourseFee { get; set; }

       
        public string Description { get; set; }
    }
}
