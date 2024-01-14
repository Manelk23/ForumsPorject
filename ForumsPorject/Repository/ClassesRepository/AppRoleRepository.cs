using Microsoft.EntityFrameworkCore;
using ForumsPorject.Repository.Entites;
using ForumsPorject.Repository.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace ForumsPorject.Repository.ClassesRepository
{
    public class AppRoleRepository : IRepository<AppRole>
    {
        private readonly DB_ForumsDbContext _context;

        public AppRoleRepository(DB_ForumsDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(AppRole entity)
        {
            await _context.AppRoles.AddAsync(entity);
            await _context.SaveChangesAsync();
        }

        public async Task AddRangeAsync(IEnumerable<AppRole> entities)
        {
            await _context.AppRoles.AddRangeAsync(entities);
            await _context.SaveChangesAsync();
        }

        public IQueryable<AppRole> Find(Expression<Func<AppRole, bool>> predicate)
        {
            return _context.AppRoles.Where(predicate);
        }

        public async Task<IEnumerable<AppRole>> GetAllAsync()
        {
            return await _context.AppRoles.ToListAsync();
        }

        public async Task<AppRole> GetByIdAsync(int id)
        {
            return await _context.AppRoles.FindAsync(id);
        }

        public void Remove(AppRole entity)
        {
            _context.AppRoles.Remove(entity);
            _context.SaveChanges();
        }

        public void RemoveRange(IEnumerable<AppRole> entities)
        {
            _context.AppRoles.RemoveRange(entities);
            _context.SaveChanges();
        }

        public void Update(AppRole entity)
        {
            _context.AppRoles.Update(entity);
            _context.SaveChanges();
        }

        public async Task UpdateAsync(AppRole entity)
        {
            _context.AppRoles.Update(entity);
            await _context.SaveChangesAsync();
        }

        public void UpdateRange(IEnumerable<AppRole> entities)
        {
            throw new NotImplementedException();
        }


        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }


        public async Task<AppRole> GetBySimpleRoleAsync(string simpleRole)
        {
            // Recherche le rôle dans la base de données
            var appRole = await _context.AppRoles
            .Where(ar => ar.SimpleRole == simpleRole || (ar.SimpleRole == null && simpleRole == null))
            .FirstOrDefaultAsync();

            // Renvoie le rôle
            return appRole;
        }
        public async Task AddAppRoleToUtilisateurAsync(AppRole appRole, Utilisateur utilisateur)
        {
            // Création de l'association entre l'utilisateur et le rôle
            var utilisateurRoles = new UtilisateurRole
            {
                UtilisateurID = utilisateur.UtilisateurId,
                AppRoleId = appRole.AppRoleId
            };

            await _context.AddAsync(utilisateurRoles);
            await _context.SaveChangesAsync();
        }
        public async Task<AppRole> CreateDefaultRoleAsync()
        {
            var defaultRole = new AppRole { SimpleRole = "SimpleUser" };
            await _context.AppRoles.AddAsync(defaultRole);
            await _context.SaveChangesAsync();
            return defaultRole;
        }


    }
}
