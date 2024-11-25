using MSS1.DTOs.RequestDTOs;
using MSS1.DTOs.ResponseDTOs;
using MSS1.Entities;
using MSS1.Interfaces;
using NuGet.Protocol.Core.Types;

namespace MSS1.Services
{
    public class ContactUsService:IContactUsService
    {
        private readonly IContactUsRepository _contactUsRepository;

        public ContactUsService(IContactUsRepository contactUsRepository)
        {
            _contactUsRepository = contactUsRepository;
        }

        public async Task<ContactUsResponseDTO> SubmitContactAsync(ContactUsRequestDTO request)
        {
            var contact = new ContactUs
            {
                FullName = request.FullName,
                EmailAddress = request.EmailAddress,
                Message = request.Message
            };

            var savedContact = await _contactUsRepository.AddAsync(contact);

            return new ContactUsResponseDTO
            {
                ContactId = savedContact.ContactId,
                FullName = savedContact.FullName,
                EmailAddress = savedContact.EmailAddress,
                Message = savedContact.Message,
                SubmittedOn = savedContact.SubmittedOn
            };
        }
    }
}
