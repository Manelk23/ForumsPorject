using ForumsPorject.Controllers;
using ForumsPorject.Repository.ClassesRepository;
using ForumsPorject.Repository.Entites;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;

namespace ForumsPorject.Services
{
    public class ThemeService
    {
        private readonly ThemeRepository _themeRepository;
        private readonly ForumRepository _forumRepository;
        private readonly IHubContext<ForumHub> _hubContext;

        public ThemeService(ThemeRepository themeRepository, ForumRepository forumRepository, IHubContext<ForumHub> hubContext)
        {
            _themeRepository = themeRepository;
            _forumRepository = forumRepository;
            _hubContext = hubContext;
        }

        public async Task<IEnumerable<Theme>> GetThemesByForumIdAsync(int forumId)
        {
            // Vérifier si le forum existe
            var forum = await _forumRepository.GetByIdAsync(forumId);

            if (forum == null)
            {
                throw new EntityNotFoundException("Forum not found");
            }

            // Récupérer la liste des forums pour la catégorie spécifiée
            var themes = await _themeRepository.Find(TH => TH.Forumid == forumId).ToListAsync();

            return themes;
        }



        // utilisee par la methode Create
        public async Task<Theme> CreateThemeAsync(string titre, DateTime date, int? idforum)
        {

            // Mapper le modèle à une entité message 
            var theme = new Theme
            {
                TitreTheme = titre,
                DateCreationTheme = date,
                Forumid = idforum ?? 0,

                // Assurez-vous de mapper d'autres propriétés au besoin...
            };
            await _themeRepository.AddAsync(theme);


            // Broadcast a message to all connected clients in the same discussion group
            await _hubContext.Clients
              .Group(theme.Forumid.ToString())
              .SendAsync("NewMessage", theme);

            return theme;
        }
        

        public async Task<Theme> GetThemeByIdAsync(int id)
        {
            var theme = await _themeRepository.GetByIdAsync(id);

            if (theme == null)
            {
                throw new EntityNotFoundException("Theme not found");
            }



            return theme;
        }
        // utilise par la methode 
        public async Task<bool> UpdateThemeAsync(int themeId, string nouveauContenu)
        {
            // Récupérer le message à mettre à jour
            var theme = await _themeRepository.GetByIdAsync(themeId);

            // Vérifier si le message existe et si l'utilisateur est l'auteur du message
            if (theme != null)
            {
                // Mettre à jour le contenu du message
                theme.TitreTheme = nouveauContenu;

                // Appeler la méthode de mise à jour du repository
                _themeRepository.UpdateAsync(theme);

                return true; // Mise à jour réussie
            }

            return false; // Message non trouvé ou utilisateur non autorisé
        }
        public async Task RemoveTheme(int themeId, string Contenu, DateTime date)
        {
            // Récupérer le theme à mettre à jour
            var theme = await _themeRepository.GetByIdAsync(themeId);
            await _themeRepository.RemoveAsyn(theme);
            await _hubContext.Clients
                   .Group(theme.Forumid.ToString())
                   .SendAsync("NewMessage", theme);
        }

    }

}

