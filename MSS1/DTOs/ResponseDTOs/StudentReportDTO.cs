using MSS1.DTOs.RequestDTOs;

namespace MSS1.DTOs.ResponseDTOs
{
    public class StudentReportDTO
    {
        public int StudentId { get; set; }
        public string StudentName { get; set; }
        public string NIC { get; set; }
        public List<CourseReportDTO> Courses { get; set; }
        public PaymentDetailsDTO PaymentDetails { get; set; }
    }
}
