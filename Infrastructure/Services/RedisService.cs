using System;
using System.Threading.Tasks;
using Application.Interfaces;
using StackExchange.Redis;

namespace Infrastructure.Services
{
    public class RedisService : IRedisService
    {
        private readonly IDatabase _db;

        public RedisService(IConnectionMultiplexer redis)
        {
            _db = redis.GetDatabase();
        }

        public async Task SetUrlAsync(string shortCode, string longUrl, TimeSpan? expiry = null)
        {
            if (expiry.HasValue)
            {
                await _db.StringSetAsync(shortCode, longUrl, expiry.Value);
            }
            else
            {
                await _db.StringSetAsync(shortCode, longUrl);
            }
        }

        public async Task<string?> GetUrlAsync(string shortCode)
        {
            return await _db.StringGetAsync(shortCode);
        }
    }
}
