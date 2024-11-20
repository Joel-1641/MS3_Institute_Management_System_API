using MSS1.DTOs.ResponseDTOs;
using MSS1.Interfaces;

namespace MSS1.Services
{
    public class AuthService: IAuthService
    {
        private readonly ITokenRepository _tokenRepository;

        public AuthService(ITokenRepository tokenRepository)
        {
            _tokenRepository = tokenRepository;
        }

        public void Logout(string token)
        {
            if (string.IsNullOrWhiteSpace(token))
                throw new ArgumentException("Token cannot be null or empty.", nameof(token));

            _tokenRepository.InvalidateToken(token);
        }
      
    }
}
