using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace URLShortener.BLL.DTOs
{
    public class URLInfoDTO
    {
        public Guid UrlId { get; set; }
        public string FullURL { get; set; }
        public string ShortenURL { get; set; }
        public string Token { get; set; }
        public int CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
