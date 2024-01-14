using ForumsPorject.Repository.ClassesRepository;
using ForumsPorject.Repository.Entites;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;

namespace ForumsPorject.Services
{
    public class ForumService
    {
        private readonly ForumRepository _forumRepository;
        private readonly CategoryRepository _categoryRepository;
        private readonly IHubContext<ForumHub> _hubContext;

        public ForumService(ForumRepository forumRepository, CategoryRepository categoryRepository, IHubContext<ForumHub> hubContext)
        {
            _forumRepository = forumRepository;
            _categoryRepository = categoryRepository;
            _hubContext = hubContext;
        }

        public async Task<IEnumerable<Forum>> GetForumsByCategoryIdAsync(int categoryId)
        {
            // Vérifier si la catégorie existe
            var category = await _categoryRepository.GetByIdAsync(categoryId);

            if (category == null)
            {
                throw new EntityNotFoundException("Category not found");
            }

            // Récupérer la liste des forums pour la catégorie spécifiée
            var forums = await _forumRepository.Find(f => f.Categorieid == categoryId).ToListAsync();

            return forums;
        }
        public async Task<Forum> CreateForumAsync(string titre, DateTime date, String image, int idCategorie )
        {
            // Mapper le modèle à une entité Discussion (si nécessaire)
            var forum = new Forum
            {
                TitreForum = titre,
                DateCreationForum = date,
                DiscriptionForum = image,
                Categorieid = idCategorie
                // Assurez-vous de mapper d'autres propriétés au besoin...
            };
            await _forumRepository.AddAsync(forum);
            // Appeler
            return forum;

        }
        public async Task<Forum> GetForumByIdAsync(int id)
        {
           var forum= await _forumRepository.GetByIdAsync(id);
            return forum;

        }

        public async Task<bool> UpdateForumAsync(int Id, string nouveauTitre,DateTime date, string nouvelleimage)
        {
            // Récupérer le message à mettre à jour
            var forum = await _forumRepository.GetByIdAsync(Id);

            // Vérifier si le message existe et si l'utilisateur est l'auteur du message
            if (forum != null)
            {
                // Mettre à jour le contenu du message
                forum.TitreForum = nouveauTitre;
                forum.DiscriptionForum = nouvelleimage;
                forum.DateCreationForum = date;


                // Appeler la méthode de mise à jour du repository
                await _forumRepository.UpdateAsync(forum);

                return true; // Mise à jour réussie
            }

            return false; // forum non trouvé ou utilisateur non autorié
        }

        public async Task RemoveForum(int Id, string Contenu, DateTime date, string discrip)
        {
            // Récupérer le message à mettre à jour
            var forum = await _forumRepository.GetByIdAsync(Id);
            await _forumRepository.RemoveAsyn(forum);
            await _hubContext.Clients
                   .Group(forum.ForumId.ToString())
                   .SendAsync("NewForum", forum);
        }

    }
}
