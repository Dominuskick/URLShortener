﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Entities
{
    public class URLInfo
    {
        [Key]
        public Guid urlId { get; set; }
        [Required]
        [MaxLength(250)]
        public string fullURL { get; set; }
        [Required]
        [MaxLength(250)]
        public string shortenURL { get; set; }
        [Required]
        [MaxLength(7)]
        public string token { get; set; }
        public int createdBy { get; set; }
        public DateTime createdDate { get; set; }
    }
}
