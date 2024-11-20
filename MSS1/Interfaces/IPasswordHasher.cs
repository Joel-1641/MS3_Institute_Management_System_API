namespace MSS1.Interfaces
{
    public interface IPasswordHasher
    {
        (string hashedPassword, string salt) HashPassword(string password);
        bool VerifyPassword(string password, string hashedPassword, string salt);
    }
}
