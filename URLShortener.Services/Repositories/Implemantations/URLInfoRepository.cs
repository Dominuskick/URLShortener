using DAL.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using URLShortener.DAL.Repositories.Interfaces;

namespace URLShortener.DAL.Repositories.Implemantations
{
    public class URLInfoRepository : BaseRepository<URLInfo, Guid>, IURLInfoRepository
    {
        public URLInfoRepository(DbContext context) : base(context)
        {
        }
    }
}
