using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace URLShortener.BLL.Services.Interfaces
{
    public interface IURLShortenerService
    {
        public Task<string> GenerateUniqueCodeAsync();
    }
}
