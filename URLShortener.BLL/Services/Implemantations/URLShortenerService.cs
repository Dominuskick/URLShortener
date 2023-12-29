using DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using URLShortener.BLL.DTOs;
using URLShortener.BLL.Services.Interfaces;
using URLShortener.DAL.Repositories.Interfaces;

namespace URLShortener.BLL.Services.Implemantations
{
    public class URLShortenerService : IURLShortenerService
    {
        public const int NumberOfCharsInShortLink = 7;
        private const string Alpabet = "AaBbCcDdEeFfGgHhIiJjKkLlMmNnOoPpQqRrSsTtUuVvWwXxYyZz0123456789";
        private readonly IURLInfoRepository _urlInfoRepository;

        public URLShortenerService(IURLInfoRepository urlInfoRepository)
        {
            _urlInfoRepository = urlInfoRepository;
        }

        private readonly Random _random = new Random();

        public async Task<string> GenerateUniqueCodeAsync()
        {
            var codeChars = new char[NumberOfCharsInShortLink];

            while (true)
            {
                for(int i = 0; i < NumberOfCharsInShortLink; i++)
                {
                    int randomIndex = _random.Next(Alpabet.Length - 1);

                    codeChars[i] = Alpabet[randomIndex];
                }

                var code = new string(codeChars);

                var result = await _urlInfoRepository.GetAllAsync();
                if(!result.Any(s => s.shortenURL == code))
                {
                    return code;
                }
            }
        }
    }
}
