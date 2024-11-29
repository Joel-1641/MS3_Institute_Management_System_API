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

        public async Task<IEnumerable<LecturerResponseDTO>> GetAllLecturersAsync()
        {
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

        }


    }
}
