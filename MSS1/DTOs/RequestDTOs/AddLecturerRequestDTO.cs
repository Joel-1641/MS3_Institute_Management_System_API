namespace MSS1.DTOs.RequestDTOs
{
    public class AddLecturerRequestDTO:AddUserRequestDTO
    {
        public List<string> Courses { get; set; }
    }
}
