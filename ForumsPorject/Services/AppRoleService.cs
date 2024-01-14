using Microsoft.EntityFrameworkCore;
using ForumsPorject.Repository.ClassesRepository;
using ForumsPorject.Repository.Entites;

namespace ForumsPorject.Services
{
    public class AppRoleService
    { 
        private readonly AppRoleRepository _appRoleRepository;

        public  AppRoleService(AppRoleRepository appRoleRepository)
        {
            _appRoleRepository = appRoleRepository;
        }
        //public async Task<AppRole> GetDefaultRoleAsync(string roleName)
        //{
        //    // Implement logic to get the default role from the database
        //    return await _appRoleRepository.GetDefaultRoleAsync(roleName);
        //}

        public async Task<AppRole> GetOrCreateRoleAsync(string roleName)
        {
            // Recherche le rôle dans la base de données
            var appRole = await _appRoleRepository.GetBySimpleRoleAsync(roleName);

            // Si le rôle n'existe pas, il est créé
            if (appRole == null)
            {
                appRole = new AppRole
                {
                    SimpleRole = roleName
                };

                await _appRoleRepository.AddAsync(appRole);
            }

            return appRole;
        }

    }
    }



