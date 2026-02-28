using System.Threading.Tasks;
using Application.DTOs;

namespace Application.Interfaces
{
    public interface IUrlShortenerService
    {
        Task<UrlResponseDto> CreateShortUrlAsync(UrlRequestDto request, string baseUrl);
        Task<string> GetLongUrlAsync(string shortCode);
        Task<UrlStatsDto> GetUrlStatsAsync(string shortCode);
    }
}
