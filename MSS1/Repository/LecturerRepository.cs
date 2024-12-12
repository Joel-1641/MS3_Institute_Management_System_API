using Microsoft.EntityFrameworkCore;
using MSS1.Database;
using MSS1.Entities;
using MSS1.Interfaces;

namespace MSS1.Repository
{
    public class LecturerRepository : ILecturerRepository
    {
        private readonly ITDbContext _context;

        public LecturerRepository(ITDbContext context)
        {
            _context = context;
        }

        //public async Task AddLecturerAsync(Lecturer lecturer)
        //{
        //    await _context.Lecturers.AddAsync(lecturer);
        //    await _context.SaveChangesAsync();
        //}
        public async Task<List<Lecturer>> GetAllLecturersAsync()
        {
            return await _context.Lecturers
                .Include(l => l.User) // Include User details
                .ThenInclude(u => u.Role) // Include Role details
                .Include(l => l.Courses) // Include related Courses
                .ToListAsync();
        }


        public async Task<Lecturer> GetLecturerByIdAsync(int lecturerId)
        {
            return await _context.Lecturers
                .Include(l => l.User)
                .Include(l => l.Courses) // Include Courses
                .FirstOrDefaultAsync(l => l.LecturerId == lecturerId);
        }

        public async Task DeleteLecturerAsync(int lecturerId)
        {
            var lecturer = await _context.Lecturers
                .Include(l => l.User) // Include the User entity
                .FirstOrDefaultAsync(l => l.LecturerId == lecturerId);

            if (lecturer == null)
                throw new ArgumentException("Lecturer not found.");

            _context.Users.Remove(lecturer.User); // Remove the User
            _context.Lecturers.Remove(lecturer); // Remove the Lecturer
            await _context.SaveChangesAsync();
        }



        public async Task UpdateLecturerAsync(Lecturer lecturer)
        {
            _context.Lecturers.Update(lecturer);
            await _context.SaveChangesAsync();
        }
        public async Task<int> GetTotalLecturerCountAsync()
        {
            return await _context.Lecturers.CountAsync();
        }
    }

}
