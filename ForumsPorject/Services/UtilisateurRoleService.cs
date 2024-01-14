using ForumsPorject.Repository.ClassesRepository;

namespace ForumsProject.Services
{
    public class UtilisateurRoleService
    {
        private readonly UtilisateurRoleRepository _roleRepository;
        public UtilisateurRoleService(UtilisateurRoleRepository roleRepository)
        {
            _roleRepository = roleRepository;
        }
        public async Task RemoveRolesByUtilisateurIdAsync(int utilisateurId)
        {
            await _roleRepository.RemoveByUtilisateurIdAsync(utilisateurId);
        }

    }
}
