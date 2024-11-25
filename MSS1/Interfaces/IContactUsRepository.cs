using MSS1.Entities;

namespace MSS1.Interfaces
{
    public interface IContactUsRepository
    {
        Task<ContactUs> AddAsync(ContactUs contact);
    }
}
