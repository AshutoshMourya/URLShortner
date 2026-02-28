using System;
using System.Threading.Tasks;

namespace Application.Interfaces
{
    public interface IRedisService
    {
        Task SetUrlAsync(string shortCode, string longUrl, TimeSpan? expiry = null);
        Task<string?> GetUrlAsync(string shortCode);
    }
}
