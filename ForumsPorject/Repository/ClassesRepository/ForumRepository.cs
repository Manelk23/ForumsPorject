
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using ForumsPorject.Repository;
using ForumsPorject.Repository.Entites;
using ForumsPorject.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace ForumsPorject.Repository.ClassesRepository
{
    public class ForumRepository : IRepository<Forum>
    {
        private readonly DB_ForumsDbContext _context;

        public ForumRepository(DB_ForumsDbContext context)
        {
            _context = context;
        }

        public async Task<Forum> GetByIdAsync(int id)
        {
            var forum = await _context.Set<Forum>().FindAsync(id);

            if (forum == null)
            {
                throw new EntityNotFoundException("Forum not found");
            }

            return forum;
        }

        public async Task<IEnumerable<Forum>> GetAllAsync()
        {
            return await _context.Set<Forum>().ToListAsync();
        }

        public IQueryable<Forum> Find(Expression<Func<Forum, bool>> predicate)
        {
            return _context.Set<Forum>().Where(predicate);
        }

        public async Task AddAsync(Forum entity)
        {
            await _context.Set<Forum>().AddAsync(entity);
            await _context.SaveChangesAsync();
        }

        public async Task AddRangeAsync(IEnumerable<Forum> entities)
        {
            await _context.Set<Forum>().AddRangeAsync(entities);
        }

        public void Update(Forum entity)
        {
            _context.Entry(entity).State = EntityState.Modified;
        }

        public void UpdateRange(IEnumerable<Forum> entities)
        {
            foreach (var entity in entities)
            {
                _context.Entry(entity).State = EntityState.Modified;
            }
        }
        public async Task RemoveAsyn(Forum entity)
        {

            var messagesASupprimer = _context.Messages.Where(m => m.Discussion.Theme.Forumid == entity.ForumId);
            _context.Messages.RemoveRange(messagesASupprimer);

            var discussionsASupprimer = _context.Discussions.Where(d => d.Theme.Forumid == entity.ForumId);
            _context.Discussions.RemoveRange(discussionsASupprimer);

            var ThemesASupprimer = _context.Themes.Where(t => t.Forumid == entity.ForumId);
            _context.Themes.RemoveRange(ThemesASupprimer);


            _context.Set<Forum>().Remove(entity);
            await _context.SaveChangesAsync();
        }
        public void Remove(Forum entity)
        {
            _context.Set<Forum>().Remove(entity);
        }

        public void RemoveRange(IEnumerable<Forum> entities)
        {
            _context.Set<Forum>().RemoveRange(entities);
        }
        public async Task UpdateAsync(Forum entity)
        {
            _context.Entry(entity).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }
       
    }
}

