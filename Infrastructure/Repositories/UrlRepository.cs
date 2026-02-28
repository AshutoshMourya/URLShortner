using System.Threading.Tasks;
using Application.Interfaces;
using Domain.Entities;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    public class UrlRepository : IUrlRepository
    {
        private readonly ApplicationDbContext _context;

        public UrlRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<UrlMapping?> GetByShortCodeAsync(string shortCode)
        {
            return await _context.UrlMappings
                .FirstOrDefaultAsync(u => u.ShortCode == shortCode);
        }

        public async Task<bool> ExistsByShortCodeAsync(string shortCode)
        {
            return await _context.UrlMappings
                .AnyAsync(u => u.ShortCode == shortCode);
        }

        public async Task AddAsync(UrlMapping urlMapping)
        {
            _context.UrlMappings.Add(urlMapping);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(UrlMapping urlMapping)
        {
            _context.UrlMappings.Update(urlMapping);
            await _context.SaveChangesAsync();
        }
    }
}
