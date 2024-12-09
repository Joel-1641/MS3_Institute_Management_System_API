namespace MSS1.DTOs.ResponseDTOs
{
    public class CourseResponseDTO
    {
        public int CourseId { get; set; }
        public string CourseName { get; set; }
        public string Level { get; set; }
        public decimal CourseFee { get; set; }
        public string Description { get; set; }
        public string CourseDuration { get; set; }
        public string CourseImg {  get; set; }
        public string CourseType { get; set; }
        public DateTime EnrollDate { get; set; } 
       // public DateTime CourseStartDate { get; set; } = DateTime.Now;
        //public DateTime CourseEndDate { get; set; }
    }
}
