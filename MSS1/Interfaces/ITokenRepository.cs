namespace MSS1.Interfaces
{
    public interface ITokenRepository
    {
        Task InvalidateTokenAsync(string token);
        Task<bool> IsTokenInvalidatedAsync(string token);
    }
}
