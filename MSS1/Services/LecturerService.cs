using MSS1.DTOs.ResponseDTOs;
using MSS1.Entities;
using MSS1.Interfaces;

namespace MSS1.Services
{
    public class LecturerService : ILecturerService
    {
        private readonly ILecturerRepository _lecturerRepository;

        public LecturerService(ILecturerRepository lecturerRepository)
        {
            _lecturerRepository = lecturerRepository;
        }

        public async Task<IEnumerable<LecturerResponseDTO>> GetAllLecturersAsync()
        {
            var lecturers = await _lecturerRepository.GetAllLecturersAsync();
            return lecturers.Select(l => new LecturerResponseDTO
            {
               // LecturerId = l.LecturerId,
                FullName = l.User.FullName,
                Email = l.User.Email,
               // NICNumber = l.User.NICNumber
            });
        }


        //public async Task<LecturerResponseDTO> GetLecturerByIdAsync(int lecturerId)
        //{
        //    var lecturer = await _lecturerRepository.GetLecturerByIdAsync(lecturerId);
        //    if (lecturer == null)
        //    {
        //        throw new KeyNotFoundException("Lecturer not found.");
        //    }

        //    return new LecturerResponseDTO
        //    {
        //        LecturerId = lecturer.LecturerId,
        //        FullName = lecturer.User.FullName,
        //        Email = lecturer.User.Email,
        //        NICNumber = lecturer.User.NICNumber,
        //        Courses = lecturer.Courses.Select(c => c.CourseName).ToList()
        //    };
        //}

        //public async Task<List<string>> GetCoursesByLecturerAsync(int lecturerId)
        //{
        //    return (await _lecturerRepository.GetCoursesByLecturerAsync(lecturerId))
        //        .Select(c => c.CourseName)
        //        .ToList();
        //}

        //public async Task AssignCoursesToLecturerAsync(int lecturerId, List<string> courses)
        //{
        //    var lecturer = await _lecturerRepository.GetLecturerByIdAsync(lecturerId);
        //    if (lecturer == null)
        //    {
        //        throw new KeyNotFoundException("Lecturer not found.");
        //    }

        //    lecturer.Courses = courses.Select(course => new LecturerCourse
        //    {
        //        CourseName = course,
        //        LecturerId = lecturerId
        //    }).ToList();

        //    await _lecturerRepository.UpdateLecturerAsync(lecturer);
        //}

        //public async Task DeleteLecturerAsync(int lecturerId)
        //{
        //    var lecturerExists = await _lecturerRepository.LecturerExistsAsync(lecturerId);
        //    if (!lecturerExists)
        //    {
        //        throw new KeyNotFoundException("Lecturer not found.");
        //    }

        //    await _lecturerRepository.DeleteLecturerAsync(lecturerId);
        //}
    }
}
