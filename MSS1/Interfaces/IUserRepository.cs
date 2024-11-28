using MSS1.Entities;

namespace MSS1.Interfaces
{
    public interface IUserRepository
    {
        //Task<IEnumerable<User>> GetAllUsersAsync();
        //Task<User> AddUserAsync(User user);
        //Task<bool> IsEmailExistsAsync(string email);
        //Task<User> GetUserByIdAsync(int userId);
        //Task<User> UpdateUserAsync(User user);
        //Task<bool> IsEmailExistsAsyncById(string email, int excludeUserId);
        //Task<User> GetUsersByIdAsync(int userId);
        //Task<bool> DeleteUserAsync(int userId);
        //Task<bool> IsUserExistsAsync(int userId);
       
            Task<bool> IsNICExistsAsync(string nicNumber);
        Task<bool> IsEmailExistsAsync(string email);
            Task<User> AddUserAsync(User user);
        //Task<User> GetUserByIdAsync(int userId);
      //  Task<IEnumerable<User>> GetAllUsersAsync();
        Task<Student> AddStudentAsync(Student student);
        Task<Lecturer> AddLecturerAsync(Lecturer lecturer);



    }
}
