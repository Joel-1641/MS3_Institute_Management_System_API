namespace MSS1.Interfaces
{
    public interface IPasswordHasher
    {
        
            (string Hash, string Salt) HashPassword(string password);
        

        bool VerifyPassword(string password, string hashedPassword, string salt);
       // string HashPassword(string password);
    }
}
