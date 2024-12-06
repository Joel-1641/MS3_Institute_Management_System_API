namespace MSS1.DTOs.RequestDTOs
{
    public class CourseDTO
    {
        public int CourseId { get; set; }
        public string CourseName { get; set; }
        public string Level { get; set; }
        public decimal CourseFee { get; set; }
        public string CourseImg {  get; set; }
        public DateTime CourseStartDate { get; set; } = DateTime.Now;
        public DateTime CourseEndDate { get; set; }
    }
}
