using AutoMapper;
using DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using URLShortener.BLL.DTOs;
using URLShortener.BLL.Services.Interfaces;
using URLShortener.DAL.Repositories.Implemantations;
using URLShortener.DAL.Repositories.Interfaces;

namespace URLShortener.BLL.Services.Implemantations
{
    public class URLInfoService : IURLInfoService
    {
        private readonly IURLInfoRepository urlInfoRepository;
        private const int pageSize = 10;
        private readonly IMapper mapper;

        public URLInfoService(IURLInfoRepository urlInfoRepository)
        {
            this.urlInfoRepository = urlInfoRepository;
            mapper = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<URLInfo, URLInfoDTO>();
                cfg.CreateMap<URLInfoDTO, URLInfo>();
            }).CreateMapper();
        }

        public async Task<IEnumerable<URLInfoDTO>> GetURLInfoDTOAsync(int pageNumber = 0)
        {
            var result = await urlInfoRepository.GetAllAsync();

            return mapper.Map<IEnumerable<URLInfo>, List<URLInfoDTO>>(result);
        }

        public async Task CreateURLInfoDTOAsync(URLInfoDTO urlInfo)
        {
            var result = mapper.Map<URLInfoDTO, URLInfo>(urlInfo);

            await urlInfoRepository.CreateAsync(result);
        }

        public async Task<URLInfoDTO> GetURLInfoDTOByIdAsync(Guid guid)
        {
            var result = await urlInfoRepository.GetAsync(guid);
            return mapper.Map<URLInfo, URLInfoDTO>(result);
        }

        public async Task<URLInfoDTO> GetURLInfoDTOByTokenAsync(string token)
        {
            var result = await urlInfoRepository.GetAllAsync();
            var urlInfo = result.FirstOrDefault(s => s.token == token);
            return mapper.Map<URLInfo, URLInfoDTO>(urlInfo);
        }

        public async Task UpdateURLInfoDTOAsync(URLInfoDTO urlInfo)
        {
            var mapped = mapper.Map<URLInfoDTO, URLInfo>(urlInfo);
            await urlInfoRepository.UpdateAsync(mapped);
        }

        public async Task DeleteURLInfoDTOAsync(Guid urlId)
        {
            await urlInfoRepository.DeleteAsync(urlId);
        }
    }
}
