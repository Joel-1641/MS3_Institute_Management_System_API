using MSS1.Database;
using MSS1.Entities;
using MSS1.Interfaces;

namespace MSS1.Repository
{
    public class ContactUsRepository: IContactUsRepository
    {
        private readonly ITDbContext _dbContext;

        public ContactUsRepository(ITDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<ContactUs> AddAsync(ContactUs contact)
        {
            _dbContext.ContactUs.Add(contact);
            await _dbContext.SaveChangesAsync();
            return contact;
        }

        //public async Task<ContactUs> GetContactDetails()
        //{
        //    _dbContext.ContactUs.
        //}

    }
}
