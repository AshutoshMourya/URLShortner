using System;
using System.Linq;
using System.Threading.Tasks;
using Application.Interfaces;

namespace Infrastructure.Services
{
    public class ShortCodeGenerator : IShortCodeGenerator
    {
        private const string Characters = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
        private readonly Random _random = new Random();

        public Task<string> GenerateUniqueCodeAsync()
        {
            var length = 7;
            var code = new string(Enumerable.Repeat(Characters, length)
              .Select(s => s[_random.Next(s.Length)]).ToArray());

            return Task.FromResult(code);
        }
    }
}
