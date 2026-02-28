using System;
using System.Threading.Tasks;
using Application.DTOs;
using Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    [ApiController]
    [Route("")]
    public class UrlController : ControllerBase
    {
        private readonly IUrlShortenerService _urlShortenerService;

        public UrlController(IUrlShortenerService urlShortenerService)
        {
            _urlShortenerService = urlShortenerService;
        }

        [HttpPost("api/url")]
        public async Task<IActionResult> CreateShortUrl([FromBody] UrlRequestDto request)
        {
            if (string.IsNullOrWhiteSpace(request.LongUrl))
            {
                return BadRequest("LongUrl is required.");
            }

            if (!Uri.TryCreate(request.LongUrl, UriKind.Absolute, out _))
            {
                return BadRequest("Invalid URL format.");
            }

            try
            {
                var baseUrl = $"{Request.Scheme}://{Request.Host}{Request.PathBase}";
                var response = await _urlShortenerService.CreateShortUrlAsync(request, baseUrl);
                return StatusCode(201, response);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception)
            {
                return StatusCode(500, "An error occurred while creating the short URL.");
            }
        }

        [HttpGet("{shortCode}")]
        public async Task<IActionResult> RedirectToUrl(string shortCode)
        {
            try
            {
                var longUrl = await _urlShortenerService.GetLongUrlAsync(shortCode);
                return Redirect(longUrl);
            }
            catch (System.Collections.Generic.KeyNotFoundException)
            {
                return NotFound();
            }
            catch (Exception)
            {
                return StatusCode(500, "An error occurred while redirecting.");
            }
        }

        [HttpGet("api/url/{shortCode}/stats")]
        public async Task<IActionResult> GetUrlStats(string shortCode)
        {
            try
            {
                var stats = await _urlShortenerService.GetUrlStatsAsync(shortCode);
                return Ok(stats);
            }
            catch (System.Collections.Generic.KeyNotFoundException)
            {
                return NotFound();
            }
            catch (Exception)
            {
                return StatusCode(500, "An error occurred while fetching stats.");
            }
        }
    }
}
