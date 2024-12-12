using MSS1.DTOs.RequestDTOs;
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

        public async Task<List<LecturerResponseDTO>> GetAllLecturersAsync()
        {
            var lecturers = await _lecturerRepository.GetAllLecturersAsync();

            return lecturers.Select(l => new LecturerResponseDTO
            {
                LecturerId = l.LecturerId,
                FullName = l.User.FullName,
                NICNumber = l.User.NICNumber,
                Email = l.User.Email,
                Gender = l.User.Gender,
                Address = l.User.Address,
                MobileNumber = l.User.MobileNumber,
                DateOfBirth = l.User.DateOfBirth,
                ProfilePicture = l.User.ProfilePicture,
                // RoleName = l.User.Role?.RoleName, // Map the RoleName here
                Courses = l.Courses.Select(c => c.CourseName).ToList()
            }).ToList();
        }

        public async Task<LecturerResponseDTO> GetLecturerByIdAsync(int lecturerId)
        {
            var lecturer = await _lecturerRepository.GetLecturerByIdAsync(lecturerId);
            if (lecturer == null) return null;

            return new LecturerResponseDTO
            {
                UserId = lecturer.User.UserId,
                FullName = lecturer.User.FullName,
                Email = lecturer.User.Email,
                //RoleName = "Lecturer",
                ProfilePicture = lecturer.User.ProfilePicture,
                LecturerId = lecturer.LecturerId,
                Courses = lecturer.Courses.Select(c => c.CourseName).ToList()
            };
        }


        public async Task DeleteLecturerAsync(int lecturerId)
        {
            await _lecturerRepository.DeleteLecturerAsync(lecturerId);
        }

        public async Task UpdateLecturerAsync(int lecturerId, UpdateLecturerRequestDTO request)
        {
            var lecturer = await _lecturerRepository.GetLecturerByIdAsync(lecturerId);
            if (lecturer == null) throw new ArgumentException("Lecturer not found.");

            // Update User details
            lecturer.User.FullName = request.FullName;
            lecturer.User.Email = request.Email;
            lecturer.User.NICNumber = request.NICNumber;
            lecturer.User.Gender = request.Gender;
            lecturer.User.Address = request.Address;
            lecturer.User.MobileNumber = request.MobileNumber;
            lecturer.User.ProfilePicture = request.ProfilePicture;
            lecturer.User.DateOfBirth = request.DateOfBirth;

            // Update Courses
            lecturer.Courses = request.Courses.Select(course => new LecturerCourse { CourseName = course }).ToList();

            await _lecturerRepository.UpdateLecturerAsync(lecturer);
        }
        public async Task<int> GetTotalLecturerCountAsync()
        {
            return await _lecturerRepository.GetTotalLecturerCountAsync();
        }


    }
}
