using AutoMapper;
using DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using URLShortener.BLL.DTOs;
using URLShortener.DAL.Repositories.Implemantations;

namespace URLShortener.BLL.Services.Interfaces
{
    public interface IURLInfoService
    {
        public Task<IEnumerable<URLInfoDTO>> GetURLInfoDTOAsync(int pageNumber = 0);

        public Task CreateURLInfoDTOAsync(URLInfoDTO urlInfo);

        public Task<URLInfoDTO> GetURLInfoDTOByIdAsync(Guid guid);

        public Task UpdateURLInfoDTOAsync(URLInfoDTO urlInfo);
        public Task DeleteURLInfoDTOAsync(Guid urlId);
        public Task<URLInfoDTO> GetURLInfoDTOByTokenAsync(string token);
    }
}
