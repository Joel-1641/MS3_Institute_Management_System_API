namespace MSS1.DTOs.ResponseDTOs
{
    public class LecturerResponseDTO : UserResponseDTO
    {
        public int LecturerId { get; set; } // Specific to the Lecturer entity
        public List<string> Courses { get; set; }
    }

}
