using System;
using System.Threading.Tasks;
using Application.DTOs;
using Application.Interfaces;
using Domain.Entities;

namespace Application.Services
{
    public class UrlShortenerService : IUrlShortenerService
    {
        private readonly IUrlRepository _urlRepository;
        private readonly IRedisService _redisService;
        private readonly IShortCodeGenerator _shortCodeGenerator;

        public UrlShortenerService(
            IUrlRepository urlRepository,
            IRedisService redisService,
            IShortCodeGenerator shortCodeGenerator)
        {
            _urlRepository = urlRepository;
            _redisService = redisService;
            _shortCodeGenerator = shortCodeGenerator;
        }

        public async Task<UrlResponseDto> CreateShortUrlAsync(UrlRequestDto request, string baseUrl)
        {
            string shortCode;

            if (!string.IsNullOrWhiteSpace(request.CustomAlias))
            {
                if (await _urlRepository.ExistsByShortCodeAsync(request.CustomAlias))
                {
                    throw new InvalidOperationException("Custom alias already in use.");
                }
                shortCode = request.CustomAlias;
            }
            else
            {
                while (true)
                {
                    shortCode = await _shortCodeGenerator.GenerateUniqueCodeAsync();
                    if (!await _urlRepository.ExistsByShortCodeAsync(shortCode))
                    {
                        break;
                    }
                }
            }

            var urlMapping = new UrlMapping
            {
                Id = Guid.NewGuid(),
                LongUrl = request.LongUrl,
                ShortCode = shortCode,
                CreatedAt = DateTime.UtcNow,
                LastAccessedAt = DateTime.UtcNow
            };

            await _urlRepository.AddAsync(urlMapping);
            
            // Cache the newly created mapping with 24 hours TTL
            await _redisService.SetUrlAsync(shortCode, request.LongUrl, TimeSpan.FromHours(24));

            return new UrlResponseDto
            {
                ShortUrl = $"{baseUrl.TrimEnd('/')}/{shortCode}"
            };
        }

        public async Task<string> GetLongUrlAsync(string shortCode)
        {
            // 1. Check Redis
            var longUrl = await _redisService.GetUrlAsync(shortCode);
            
            UrlMapping? urlMapping = null;

            if (string.IsNullOrEmpty(longUrl))
            {
                // 2. Check Database if missing in Redis
                urlMapping = await _urlRepository.GetByShortCodeAsync(shortCode);
                
                if (urlMapping == null)
                {
                    throw new System.Collections.Generic.KeyNotFoundException("Short URL not found.");
                }

                longUrl = urlMapping.LongUrl;
                
                // 3. Update Redis
                await _redisService.SetUrlAsync(shortCode, longUrl, TimeSpan.FromHours(24));
            }
            else
            {
                // Fetch from DB just to update stats if necessary, but this could be done async
                // For accuracy, we'll fetch and update stats.
                urlMapping = await _urlRepository.GetByShortCodeAsync(shortCode);
            }

            if (urlMapping != null)
            {
                urlMapping.ClickCount++;
                urlMapping.LastAccessedAt = DateTime.UtcNow;
                await _urlRepository.UpdateAsync(urlMapping);
            }

            return longUrl;
        }

        public async Task<UrlStatsDto> GetUrlStatsAsync(string shortCode)
        {
            var urlMapping = await _urlRepository.GetByShortCodeAsync(shortCode);
            
            if (urlMapping == null)
            {
                throw new System.Collections.Generic.KeyNotFoundException("Short URL not found.");
            }

            return new UrlStatsDto
            {
                LongUrl = urlMapping.LongUrl,
                ShortCode = urlMapping.ShortCode,
                ClickCount = urlMapping.ClickCount,
                CreatedAt = urlMapping.CreatedAt,
                LastAccessedAt = urlMapping.LastAccessedAt
            };
        }
    }
}
