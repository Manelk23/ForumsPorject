
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using ForumsPorject.Repository;
using ForumsPorject.Repository.ClassesRepository;
using ForumsPorject.Repository.Entites;
using ForumsPorject.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace ForumsPorject.Repository.ClassesRepository
{
    public class ThemeRepository : IRepository<Theme>
    {
        private readonly DB_ForumsDbContext _context;

        public ThemeRepository(DB_ForumsDbContext context)
        {
            _context = context;
        }

        public async Task<Theme> GetByIdAsync(int id)
        {
            var theme = await _context.Set<Theme>().FindAsync(id);

            if (theme == null)
            {
                throw new EntityNotFoundException("Theme not found");
            }

            return theme;
        }

        public async Task<IEnumerable<Theme>> GetAllAsync()
        {
            return await _context.Set<Theme>().ToListAsync();
        }

        public IQueryable<Theme> Find(Expression<Func<Theme, bool>> predicate)
        {
            return _context.Set<Theme>().Where(predicate);
        }

        public async Task AddAsync(Theme entity)
        {
            await _context.Set<Theme>().AddAsync(entity);
            await _context.SaveChangesAsync();
        }

        public async Task AddRangeAsync(IEnumerable<Theme> entities)
        {
            await _context.Set<Theme>().AddRangeAsync(entities);
        }

        public void Update(Theme entity)
        {
            _context.Entry(entity).State = EntityState.Modified;
        }

        public void UpdateRange(IEnumerable<Theme> entities)
        {
            foreach (var entity in entities)
            {
                _context.Entry(entity).State = EntityState.Modified;
            }
        }

        public void Remove(Theme entity)
        {
            _context.Set<Theme>().Remove(entity);
        }

        public void RemoveRange(IEnumerable<Theme> entities)
        {
            _context.Set<Theme>().RemoveRange(entities);
        }

        public async Task UpdateAsync(Theme entity)
        {
            _context.Entry(entity).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }
        public async Task RemoveAsyn(Theme entity)
        {
            _context.Set<Theme>().Remove(entity);
            await _context.SaveChangesAsync();
        }
    }
}



