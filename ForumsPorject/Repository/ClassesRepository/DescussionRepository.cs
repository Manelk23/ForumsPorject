
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
        public IQueryable<Discussion> Find(Expression<Func<Discussion, bool>> predicate)
        {
            return _context.Set<Discussion>().Where(predicate);
        }

        public async Task AddAsync(Discussion entity)
        {

            await _context.Discussions.AddAsync(entity);
            await _context.SaveChangesAsync();
        }
        public async Task<Discussion> GetByIdAsync(int id)
        {
            return await _context.Set<Discussion>().FindAsync(id);
        }
        public async Task UpdateAsync(Discussion entity)
        {
            _context.Set<Discussion>().Update(entity);
            await _context.SaveChangesAsync();
        }

        public async Task RemoveAsyn(Discussion entity)
        {
            _context.Set<Discussion>().Remove(entity);
            await _context.SaveChangesAsync();
        }


        public async Task<IEnumerable<Discussion>> GetAllAsync()
        {
            return await _context.Set<Discussion>().ToListAsync();
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

        public void Remove(Discussion entity)
        {
            _context.Set<Discussion>().Remove(entity);
            _context.SaveChanges();
        }
        public async Task<List<Discussion>> GetDiscussionsCreer(int utilisateurId)
        {
            // Implémentez la logique pour récupérer les discussions créées par l'utilisateur
            // Utilisez votre _descService ou tout autre mécanisme de récupération de données

            var discussions = await _context.Discussions
                .Where(d => d.Utilisateurid == utilisateurId)
                .ToListAsync();

            return discussions;
        }

    }
}