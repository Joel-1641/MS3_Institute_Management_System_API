using MSS1.Entities;

namespace MSS1.Interfaces
{
    public interface IUserRepository
    {
       
       
            Task<bool> IsNICExistsAsync(string nicNumber);
        Task<bool> IsEmailExistsAsync(string email);
            Task<User> AddUserAsync(User user);
     
        Task<Student> AddStudentAsync(Student student);
        Task<Lecturer> AddLecturerAsync(Lecturer lecturer);
      


    }
}
