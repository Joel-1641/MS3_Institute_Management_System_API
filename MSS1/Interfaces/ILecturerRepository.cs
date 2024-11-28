using MSS1.Entities;

namespace MSS1.Interfaces
{
    public interface ILecturerRepository
    {
        //Task AddLecturerAsync(Lecturer lecturer);
        Task<IEnumerable<Lecturer>> GetAllLecturersAsync();
       // Task<Lecturer> GetLecturerByIdAsync(int lecturerId);
       // Task<List<LecturerCourse>> GetCoursesByLecturerAsync(int lecturerId);
    }
}
