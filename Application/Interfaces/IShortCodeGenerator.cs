using System.Threading.Tasks;

namespace Application.Interfaces
{
    public interface IShortCodeGenerator
    {
        Task<string> GenerateUniqueCodeAsync();
    }
}
