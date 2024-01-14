using Microsoft.EntityFrameworkCore;
using ForumsPorject.Repository.Entites;
using ForumsPorject.Repository.Interfaces;
using System.Linq.Expressions;
using static ForumsPorject.Repository.ClassesRepository.UtilisateurRepository;

namespace ForumsPorject.Repository.ClassesRepository

{
    public class UtilisateurRepository : IRepository<Utilisateur>
    {
        private readonly DB_ForumsDbContext _context;
        private readonly ILogger<UtilisateurRepository> _logger;

        public UtilisateurRepository(DB_ForumsDbContext context, ILogger<UtilisateurRepository> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task AddAsync(Utilisateur entity)
        {
            await _context.Utilisateurs.AddAsync(entity);
            await _context.SaveChangesAsync();
        }

        public async Task AddRangeAsync(IEnumerable<Utilisateur> entities)
        {
            await _context.Utilisateurs.AddRangeAsync(entities);
            await _context.SaveChangesAsync();
        }

        public IQueryable<Utilisateur> Find(Expression<Func<Utilisateur, bool>> predicate)
        {
            return _context.Utilisateurs.Where(predicate);
        }

        public async Task<IEnumerable<Utilisateur>> GetAllAsync()
        {
            return await _context.Utilisateurs.ToListAsync();
        }

        public async Task<Utilisateur> GetByIdAsync(int id)
        {
            return await _context.Set<Utilisateur>().FindAsync(id);

        }

        public void Remove(Utilisateur entity)
        {
            _context.Utilisateurs.Remove(entity);
            _context.SaveChanges();
        }

        public void RemoveRange(IEnumerable<Utilisateur> entities)
        {
            _context.Utilisateurs.RemoveRange(entities);
            _context.SaveChanges();
        }

        public void Update(Utilisateur entity)
        {
            _context.Utilisateurs.Update(entity);
            _context.SaveChanges();
        }

        public async Task UpdateAsync(Utilisateur entity)
        {
            _context.Utilisateurs.Update(entity);
            await _context.SaveChangesAsync();
        }

        public void UpdateRange(IEnumerable<Utilisateur> entities)
        {
            _context.Utilisateurs.UpdateRange(entities);
            _context.SaveChanges();
        }
        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
        public async Task<Utilisateur> GetUtilisateurByEmailAndPassword(string email, string password)
        {
            try
            {
                var utilisateur = await _context.Utilisateurs
                    .FirstOrDefaultAsync(u => u.Email == email && u.Password == password);

                if (utilisateur != null)
                {
                    // Log success
                    _logger.LogInformation("Utilisateur récupéré avec succès.");
                }
                else
                {
                    // Log failure
                    _logger.LogWarning("Aucun utilisateur trouvé pour les identifiants donnés.");
                }

                return utilisateur;
            }
            catch (Exception ex)
            {
                // Log exception
                _logger.LogError(ex, "Erreur lors de la récupération de l'utilisateur.");

                throw; // Vous pouvez choisir de lever l'exception ou de la gérer selon votre logique métier.
            }
        }
        public async Task AddUtilisateurRoleAsync(UtilisateurRole utilisateurRole)
        {
            // Créez une nouvelle entrée dans la table UtilisateurRoles
            await _context.UtilisateurRoles.AddAsync(utilisateurRole);

            // Enregistrez les modifications
            await _context.SaveChangesAsync();
        }

        public async Task AddUtilisateurToRoleAsync(Utilisateur utilisateur, AppRole role)
        {
            var utilisateurRoles = new UtilisateurRole
            {
                UtilisateurID = utilisateur.UtilisateurId,
                AppRoleId = role.AppRoleId
            };

            await _context.AddAsync(utilisateurRoles);
            await _context.SaveChangesAsync();
        }

        public async Task<Utilisateur> GetUtilisateurByEmail(string email)
        {
            try
            {
                var utilisateur = await _context.Utilisateurs
                    .FirstOrDefaultAsync(u => u.Email == email);

                if (utilisateur != null)
                {
                    // Log success
                    _logger.LogInformation("Utilisateur récupéré avec succès.");
                }
                else
                {
                    // Log failure
                    _logger.LogWarning("Aucun utilisateur trouvé pour l'email donné.");
                }

                return utilisateur;
            }
            catch (Exception ex)
            {
                // Log exception
                _logger.LogError(ex, "Erreur lors de la récupération de l'utilisateur.");

                throw;
            }
        }
        public IQueryable<Utilisateur> Include(params Expression<Func<Utilisateur, object>>[] includes)
        {
            var query = _context.Set<Utilisateur>().AsQueryable();

            foreach (var include in includes)
            {
                query = query.Include(include);
            }

            return query;
        }
        public async Task<bool> IsEmailUniqueAsync(string email)
        {
            return await _context.Utilisateurs.AllAsync(u => u.Email != email);
        }

        public async Task<IList<string>> GetRolesForUserAsync(int utilisateurId)
        {
            var utilisateurRoles = await _context.UtilisateurRoles
                .Include(ur => ur.AppRole)
                .Where(ur => ur.UtilisateurID == utilisateurId)
                .Select(ur => $"{ur.AppRole.SimpleRole}, {ur.AppRole.ManagerRole ?? "ValeurParDefaut"}")  // Remplacez RoleProperty1 et RoleProperty2 par les propriétés réelles du rôle que vous souhaitez afficher
                .ToListAsync();

            return utilisateurRoles;
        }


    }
}



   


