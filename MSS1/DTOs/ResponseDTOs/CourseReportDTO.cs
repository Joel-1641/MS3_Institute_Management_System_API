namespace MSS1.DTOs.ResponseDTOs
{
    public class CourseReportDTO
    {
        public int CourseId { get; set; }
        public string CourseName { get; set; }
        public string Level { get; set; }
        public decimal CourseFee { get; set; }
        public string CourseDuration { get; set; }
        public string CourseType { get; set; }
    }
}
