using ForumsPorject.Repository.Entites;
using ForumsPorject.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace ForumsPorject.Repository.ClassesRepository
{
    public class UtilisateurRoleRepository : IRepository<UtilisateurRole>
    {
        private readonly DB_ForumsDbContext _context;
        public UtilisateurRoleRepository(DB_ForumsDbContext context)
        {
            _context = context;
        }
        public async Task AddAsync(UtilisateurRole entity)
        {
            await _context.Set<UtilisateurRole>().AddAsync(entity);
            await _context.SaveChangesAsync();
        }

        public Task AddRangeAsync(IEnumerable<UtilisateurRole> entities)
        {
            throw new NotImplementedException();
        }

        public IQueryable<UtilisateurRole> Find(Expression<Func<UtilisateurRole, bool>> predicate)
        {
            return _context.Set<UtilisateurRole>().Where(predicate);
        }
        public Task<IEnumerable<UtilisateurRole>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public Task<UtilisateurRole> GetByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public void Remove(UtilisateurRole entity)
        {
            throw new NotImplementedException();
        }

        public void RemoveRange(IEnumerable<UtilisateurRole> entities)
        {
            throw new NotImplementedException();
        }

        public void Update(UtilisateurRole entity)
        {
            throw new NotImplementedException();
        }

        public Task UpdateAsync(UtilisateurRole entity)
        {
            throw new NotImplementedException();
        }

        public void UpdateRange(IEnumerable<UtilisateurRole> entities)
        {
            throw new NotImplementedException();
        }
        public IQueryable<UtilisateurRole> Include(params Expression<Func<UtilisateurRole, object>>[] includes)
        {
            var query = _context.Set<UtilisateurRole>().AsQueryable();

            foreach (var include in includes)
            {
                query = query.Include(include);
            }

            return query;
        }

        public async Task RemoveByUtilisateurIdAsync(int utilisateurId)
        {
            var utilisateurRoles = _context.Set<UtilisateurRole>().Where(ur => ur.UtilisateurID == utilisateurId);

            // Supprimer les rôles de l'utilisateur
            _context.Set<UtilisateurRole>().RemoveRange(utilisateurRoles);

            await _context.SaveChangesAsync();
        }

    }

}




