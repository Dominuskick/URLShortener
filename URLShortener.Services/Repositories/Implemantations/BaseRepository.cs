using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using URLShortener.DAL.Repositories.Interfaces;

namespace URLShortener.DAL.Repositories.Implemantations
{
    public abstract class BaseRepository<TEntity, TKey> : IBaseRepository<TEntity, TKey> where TEntity : class
    {
        private readonly DbSet<TEntity> _set;
        private readonly DbContext _context;

        public BaseRepository(DbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _set = context.Set<TEntity>();
        }

        public virtual async Task CreateAsync(TEntity item)
        {
            await _set.AddAsync(item);
            await _context.SaveChangesAsync();
        }

        public virtual async Task DeleteAsync(TKey id)
        {
            var item = await GetAsync(id);
            if (item != null)
            {
                _set.Remove(item);
                await _context.SaveChangesAsync();
            }
        }

        public virtual async Task<IEnumerable<TEntity>> FindAsync(Func<TEntity, bool> predicate)
        {
            return await Task.Run(() => _set.Where(predicate).ToList());
        }

        public virtual async Task<TEntity> GetAsync(TKey id)
        {
            return await _set.FindAsync(id);
        }

        public virtual IQueryable<TEntity> GetAllAsQueryable()
        {
            var query = _set.AsQueryable();
            return query;
        }

        public virtual async Task<IEnumerable<TEntity>> GetAllAsync()
        {
            return await _set.ToListAsync();
        }

        public virtual async Task UpdateAsync(TEntity item)
        {
            _set.Update(item);
            await _context.SaveChangesAsync();
        }
    }
}
