using MSS1.DTOs.RequestDTOs;
using MSS1.DTOs.ResponseDTOs;

namespace MSS1.Interfaces
{
    public interface IContactUsService
    {
        Task<ContactUsResponseDTO> SubmitContactAsync(ContactUsRequestDTO request);
    }
}
