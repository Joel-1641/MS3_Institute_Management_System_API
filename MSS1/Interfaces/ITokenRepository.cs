namespace MSS1.Interfaces
{
    public interface ITokenRepository
    {
        void InvalidateToken(string token);
        Task InvalidateTokenAsync(string token);
        bool IsTokenValid(string token);

        // The method to be implemented
       // Task<bool> IsTokenInvalidatedAsync(string token);
       // Task SaveTokenAsync(string token, int userId);
       // Task InvalidateTokenAsyncUsers(string token);
    }
   
}

