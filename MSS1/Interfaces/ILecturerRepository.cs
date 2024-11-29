using MSS1.Entities;

namespace MSS1.Interfaces
{
    public interface ILecturerRepository
    {
        Task<List<Lecturer>> GetAllLecturersAsync();
        Task<Lecturer> GetLecturerByIdAsync(int lecturerId);
        Task DeleteLecturerAsync(int lecturerId);
        Task UpdateLecturerAsync(Lecturer lecturer);
    }
}
