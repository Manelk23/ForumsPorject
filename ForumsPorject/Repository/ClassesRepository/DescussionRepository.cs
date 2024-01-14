
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Threading.Tasks;
using ForumsPorject.Repository.Entites;
using ForumsPorject.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace ForumsPorject.Repository.ClassesRepository
{
        public class DescussionRepository : IRepository<Discussion>
        {
            private readonly DB_ForumsDbContext _context;

            public DescussionRepository(DB_ForumsDbContext context)
            {
                _context = context;
            }

        public async Task<Discussion> GetByIdAsync(int id)
        {
            return await _context.Set<Discussion>().FindAsync(id);
        }

        public async Task<IEnumerable<Discussion>> GetAllAsync()
        {
            return await _context.Set<Discussion>().ToListAsync();
        }

        public IQueryable<Discussion> Find(Expression<Func<Discussion, bool>> predicate)
        {
            return _context.Set<Discussion>().Where(predicate);
        }

        public async Task AddAsync(Discussion entity)
        {
           
            await _context.Discussions.AddAsync(entity);
            await _context.SaveChangesAsync();
        }

        public async Task AddDiscussionAsync(Discussion discussion)
        {
            // Ajouter la discussion à la DbSet
            _context.Discussions.Add(discussion);

            // Enregistrer les modifications dans la base de données
            await _context.SaveChangesAsync();
        }

        public async Task RemoveAsyn(Discussion entity)
        {
            _context.Set<Discussion>().Remove(entity);
            await _context.SaveChangesAsync();
        }



        public async Task AddRangeAsync(IEnumerable<Discussion> entities)
        {
            await _context.Set<Discussion>().AddRangeAsync(entities);
            await _context.SaveChangesAsync();
        }

        public void Update(Discussion entity)
        {
            _context.Set<Discussion>().Update(entity);
            _context.SaveChanges();
        }

        public async Task UpdateAsync(Discussion entity)
        {
            _context.Set<Discussion>().Update(entity);
            await _context.SaveChangesAsync();
        }

        public void UpdateRange(IEnumerable<Discussion> entities)
        {
            _context.Set<Discussion>().UpdateRange(entities);
            _context.SaveChanges();
        }

        public void Remove(Discussion entity)
        {
            _context.Set<Discussion>().Remove(entity);
            _context.SaveChanges();
        }

        public void RemoveRange(IEnumerable<Discussion> entities)
        {
            _context.Set<Discussion>().RemoveRange(entities);
            _context.SaveChanges();
        }
    }
}