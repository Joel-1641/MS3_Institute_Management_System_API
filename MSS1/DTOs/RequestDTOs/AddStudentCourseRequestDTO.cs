namespace MSS1.DTOs.RequestDTOs
{
    public class AddStudentCourseRequestDTO
    {
      // public int StudentId { get; set; }
        public List<int> CourseIds { get; set; }
        public string NIC { get; set; }

    }
}
