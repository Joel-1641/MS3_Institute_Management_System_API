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
        public async Task<IEnumerable<Lecturer>> GetAllLecturersAsync()
        {
            return await _context.Lecturers
                .Include(l => l.User) // Include User details
                .Include(l => l.Courses) // Include courses the lecturer teaches
                .ToListAsync();
        }


        //public async Task<Lecturer> GetLecturerByIdAsync(int lecturerId)
        //{
        //    return await _context.Lecturers
        //        .Include(l => l.User)
        //        .Include(l => l.Courses)
        //        .FirstOrDefaultAsync(l => l.LecturerId == lecturerId);
        //}

        //public async Task<List<LecturerCourse>> GetCoursesByLecturerAsync(int lecturerId)
        //{
        //    var lecturer = await _context.Lecturers
        //        .Include(l => l.Courses)
        //        .FirstOrDefaultAsync(l => l.LecturerId == lecturerId);

        //    if (lecturer == null)
        //    {
        //        throw new KeyNotFoundException("Lecturer not found.");
        //    }

        //    return lecturer.Courses.ToList();
        //}
    }

}
