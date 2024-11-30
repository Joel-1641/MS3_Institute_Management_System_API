namespace MSS1.DTOs.RequestDTOs
{
    public class AddStudentCourseRequestDTO
    {
       
        public string NIC { get; set; }

        
        public List<CourseSelectionDTO> Courses { get; set; }



    }
}
