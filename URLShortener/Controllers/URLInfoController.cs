using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using URLShortener.BLL.DTOs;
using URLShortener.BLL.Services.Implemantations;
using URLShortener.BLL.Services.Interfaces;

namespace URLShortener.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class URLInfoController : ControllerBase
    {
        private readonly IURLInfoService urlInfoService;
        private readonly IURLShortenerService urlShortenerService;
        private readonly IHttpContextAccessor httpContext;

        public URLInfoController(IURLInfoService urlInfoService, IURLShortenerService urlShortenerService, IHttpContextAccessor httpContext)
        {
            this.urlInfoService = urlInfoService;
            this.urlShortenerService = urlShortenerService;
            this.httpContext = httpContext;
        }
        [AllowAnonymous]
        [HttpGet] //urlinfo/
        public async Task<IEnumerable<URLInfoDTO>> GetListAsync()
        {
            var result = await urlInfoService.GetURLInfoDTOAsync();
            return result.ToArray();
        }

        [HttpGet("id/{urlId}")]  //urlinfo/id/{urlId}
        public async Task<IActionResult> GetByIdAsync(Guid urlId)
        {
            var result = await urlInfoService.GetURLInfoDTOByIdAsync(urlId);
            return Ok(result);
        }

        [HttpGet("{token}")]//urlinfo/{token}
        public async Task<IActionResult> GetShortenURLByToken(string token)
        {
            var result = await urlInfoService.GetURLInfoDTOByTokenAsync(token);
            Console.WriteLine(result.FullURL);
            if (result is null)
            {
                return NotFound();
            }
            return Redirect(result.FullURL);
        }

        [HttpPost]
        public async Task<IActionResult> CreateUrlInfo([FromBody]string url)
        {
            var token = await urlShortenerService.GenerateUniqueCodeAsync();
            URLInfoDTO urlInfoDTO = new URLInfoDTO()
            {
                UrlId = Guid.NewGuid(),
                FullURL = url,
                Token = token,
                ShortenURL = $"{httpContext.HttpContext.Request.Scheme}://{httpContext.HttpContext.Request.Host}/urlinfo/{token}",
                CreatedDate = DateTime.Now,
                CreatedBy = new Random().Next(1, 6),
            };

            await urlInfoService.CreateURLInfoDTOAsync(urlInfoDTO);
            return Ok(urlInfoDTO.ShortenURL);
        }

        [HttpDelete("{urlId}")]
        public async Task<IActionResult> DeleteUrlInfoById(Guid urlId)
        {
            await urlInfoService.DeleteURLInfoDTOAsync(urlId);
            return Ok(await GetListAsync());
        }
    }
}
