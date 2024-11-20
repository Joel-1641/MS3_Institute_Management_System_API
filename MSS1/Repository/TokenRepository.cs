using MSS1.Interfaces;
using System.Collections.Concurrent;

namespace MSS1.Repository
{
    public class TokenRepository: ITokenRepository
    {
        private readonly ConcurrentDictionary<string, DateTime> _invalidTokens = new();

        public void InvalidateToken(string token)
        {
            _invalidTokens[token] = DateTime.UtcNow;
        }

        public bool IsTokenValid(string token)
        {
            return !_invalidTokens.ContainsKey(token);
        }
    }
   
}
