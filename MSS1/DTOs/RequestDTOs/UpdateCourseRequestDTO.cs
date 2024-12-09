namespace MSS1.DTOs.RequestDTOs
{
    public class UpdateCourseRequestDTO
    {
        
         //public int CourseId { get; set; } // Required for identifying the course

        
        public string CourseName { get; set; }

        
        public string Level { get; set; } // e.g., Beginner, Intermediate, Advanced

     
        public decimal CourseFee { get; set; }

        
        public string Description { get; set; }
        public string CourseDuration { get; set; }
        public string CourseImg {  get; set; }
        public string CourseType { get; set; }
        //public DateTime CourseEndDate { get; set; }
        //public DateTime CourseStartDate { get; set; } = DateTime.Now;
    }
}
