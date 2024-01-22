using Microsoft.EntityFrameworkCore;
using ForumsPorject.Repository.ClassesRepository;
using ForumsPorject.Repository.Entites;
using System.Linq.Expressions;
using Microsoft.AspNetCore.SignalR;
using System.Collections.Generic;

namespace ForumsPorject.Services
{
    public class DescussionService
    { 
        private readonly DescussionRepository _discussionRepository;
        private readonly ThemeRepository _themeRepository;
        private readonly UtilisateurRepository _utilisateurRepository;
        private readonly IHubContext<ForumHub> _hubContext;
        public DescussionService(DescussionRepository discussionRepository, ThemeRepository themeRepository,
            UtilisateurRepository utilisateurRepository, IHubContext<ForumHub> hubContext)
        {
            _discussionRepository = discussionRepository;
            _themeRepository = themeRepository;
            _utilisateurRepository = utilisateurRepository;
            _hubContext = hubContext;   
        }

        public async Task<IEnumerable<Discussion>> GetDiscussionByThemeIdAsync(int ThemeId)
        {
            // Vérifier si la catégorie existe
            var theme = await _themeRepository.GetByIdAsync(ThemeId);

            if (theme == null)
            {
                throw new EntityNotFoundException("Theme not found");
            }

            // Récupérer la liste des discussions pour la Theme spécifiée
            var discussions = await _discussionRepository.Find(des => des.Themeid == ThemeId).ToListAsync();

            return discussions;
        }


        public async Task<Discussion> CreateDiscussionAsync(string titre, DateTime date, int? idTheme, int? idUtilisateur)
        {
            // Mapper le modèle à une entité Discussion (si nécessaire)
            var discussion = new Discussion
            {
                TitreDiscussion = titre,
                DateCreationDiscussion = date,
                Themeid = idTheme,
                Utilisateurid = idUtilisateur
                // Assurez-vous de mapper d'autres propriétés au besoin...
            };
            await _discussionRepository.AddAsync(discussion);
            // Appeler
            return discussion;

        }


        public async Task<Discussion> GetDiscussionByIdAsync(int id)
        {
            return await _discussionRepository.GetByIdAsync(id);
        }

        public async Task<bool> UpdateDiscussionAsync(int Id, string nouveauTitre)
        {
            // Récupérer le message à mettre à jour
            var discussion = await _discussionRepository.GetByIdAsync(Id);

            // Vérifier si le message existe et si l'utilisateur est l'auteur du message
            if (discussion != null)
            {
                // Mettre à jour le contenu du message
                discussion.TitreDiscussion = nouveauTitre;

                // Appeler la méthode de mise à jour du repository
                _discussionRepository.UpdateAsync(discussion);
                    await _hubContext.Clients
                   .Group(discussion.DiscussionId.ToString())
                   .SendAsync("NewDiscussion", discussion);
                return true; // Mise à jour réussie
            }

            return false; // Message non trouvé ou utilisateur non autorisé
        }

        public async Task RemoveDiscussion(int Id, string Contenu, DateTime date)
        {
            // Récupérer le message à mettre à jour
            var discussion = await _discussionRepository.GetByIdAsync(Id);
            await _discussionRepository.RemoveAsyn(discussion);
            await _hubContext.Clients
                   .Group(discussion.DiscussionId.ToString())
                   .SendAsync("NewDiscussion", discussion);
        }


        public async Task<IEnumerable<Discussion>> GetDiscussionByThemeIdAndUtilisateurIdAsync(int themeId, int utilisateurId)
        {
            // Vérifier si le theme existe
            var theme = await _themeRepository.GetByIdAsync(themeId);

            if (theme == null)
            {
                throw new EntityNotFoundException("theme not found");
            }

            // Récupérer la liste des discussion pour le theme et l'utilisateur spécifiés
            var discussions = await _discussionRepository
                .Find(dis => dis.Themeid == themeId && dis.Utilisateurid == utilisateurId)
                .ToListAsync();

            return discussions;
        }
        public async Task<List<Discussion>> GetDiscussionsCreerParUtilisateurAsync(int utilisateurId)
        {
            var discussions = await _discussionRepository.GetDiscussionsCreer(utilisateurId);
            return discussions;
        }


    }
}
