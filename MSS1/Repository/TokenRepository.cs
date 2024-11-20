using MSS1.Interfaces;
using System.Collections.Concurrent;
using System.Threading.Tasks;

namespace MSS1.Repository
{
    public class TokenRepository : ITokenRepository
    {
        private readonly ConcurrentDictionary<string, DateTime> _invalidTokens = new();

        public void InvalidateToken(string token)
        {
            _invalidTokens[token] = DateTime.UtcNow;
        }

        // Asynchronous version of InvalidateToken
        public async Task InvalidateTokenAsync(string token)
        {
            // Simulate async operation (for example, saving to a database)
            await Task.Delay(10);  // Simulate some delay, for now, replace with actual async logic
            _invalidTokens[token] = DateTime.UtcNow;  // Mark the token as invalid asynchronously
        }

        public bool IsTokenValid(string token)
        {
            return !_invalidTokens.ContainsKey(token);
        }
      
    }
}
