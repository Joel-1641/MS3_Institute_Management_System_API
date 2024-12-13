using MSS1.DTOs.RequestDTOs;

namespace MSS1.DTOs.ResponseDTOs
{
    public class ProfileResposeDTO
    {
        public int StudentId { get; set; }
        public string FullName { get; set; }
        public string ProfilePicture { get; set; }
        public List<CourseReportDTO> Courses { get; set; }
        public List<PaymentDTO> Payments { get; set; }
    }
}
