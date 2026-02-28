using System.Threading.Tasks;
using Domain.Entities;

namespace Application.Interfaces
{
    public interface IUrlRepository
    {
        Task<UrlMapping?> GetByShortCodeAsync(string shortCode);
        Task<bool> ExistsByShortCodeAsync(string shortCode);
        Task AddAsync(UrlMapping urlMapping);
        Task UpdateAsync(UrlMapping urlMapping);
    }
}
