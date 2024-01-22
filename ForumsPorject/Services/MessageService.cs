using Microsoft.AspNetCore.Mvc.ViewFeatures;
using ForumsPorject.Repository.ClassesRepository;
using System.Linq.Expressions;
using ForumsPorject.Repository.Entites;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.SignalR;


namespace ForumsPorject.Services
{
    public class MessageService
    {
        private readonly MessageRepository _messageRepository;
        private readonly UtilisateurRepository _utilisateurRepository;
        private readonly IHubContext<ForumHub> _hubContext;
        private readonly DescussionService _descussionService;
        private readonly UtilisateurService _utilisateurService;

       public MessageService(
            MessageRepository messageRepository,
            UtilisateurRepository utilisateurRepository,
            IHubContext<ForumHub> hubContext,
            DescussionService descussionService,
            UtilisateurService utilisateurService
        )
        {
            _messageRepository = messageRepository;
            _utilisateurRepository = utilisateurRepository;
            _hubContext = hubContext;
            _descussionService = descussionService;
            _utilisateurService= utilisateurService;
        }

        //utilisée par la méthode Index
        public async Task<IEnumerable<Message>> GetMessagesByDiscussionAsync(int discussionId)
        {

            var messages = _messageRepository.GetMessagesByDiscId(discussionId);

            foreach (var message in messages)
            {
                // Inclure les détails de l'auteur si nécessaire
                await IncludeAuteurDetailsAsync(message);
            }

            return messages;
        }
        // utilisée par la méthode Index
        public async Task IncludeAuteurDetailsAsync(Message message)
        {
            // Chargez l'auteur avec les détails nécessaires
            if (message.AuteurId != null)
            {
                // Assurez-vous que message.Auteur n'est pas null
                message.Auteur = message.Auteur ?? new Utilisateur();

                var utilisateur = await _utilisateurRepository.GetByIdAsync(message.AuteurId);

                // Assurez-vous que utilisateur n'est pas null
                if (utilisateur != null)
                {
                    // Assignez uniquement les propriétés nécessaires
                    message.Auteur.Pseudonyme = utilisateur.Pseudonyme;
                    message.Auteur.Cheminavatar = utilisateur.Cheminavatar;
                }
                else
                {
                    // Gérer le cas où l'utilisateur n'est pas trouvé
                    // Vous pouvez lever une exception, définir des valeurs par défaut, etc.
                    // Dans cet exemple, j'initialise les propriétés à des valeurs par défaut
                    message.Auteur.Pseudonyme = "Utilisateur Inconnu";
                    message.Auteur.Cheminavatar = "Chemin Avatar Inconnu";
                }
            }
        }

        // utilisee par la methode Create
        public async Task<Message> CreateMessageAsync(string titre, DateTime date, bool Lu, bool Archive, int idUtilisateur, int? iddiscussion)
        {

            // Mapper le modèle à une entité message 
            var message = new Message
            {
                ContenuMessage = titre,
                DatecréationMessage = date,
                Lu = false,
                Archive = false,
                AuteurId = idUtilisateur,
                Discussionid = iddiscussion ?? 0,

                // Assurez-vous de mapper d'autres propriétés au besoin...
            };
            await _messageRepository.AddAsync(message);


            // Broadcast a message to all connected clients in the same discussion group
            await _hubContext.Clients
              .Group(message.Discussionid.ToString())
              .SendAsync("NewMessage", message);

            return message;
        }

        public async Task<Message> GetMessageByIdAsync(int id)
        {
            var message = await _messageRepository.GetByIdAsync(id);

            if (message == null)
            {
                throw new EntityNotFoundException("Message not found");
            }

            // Inclure les détails de l'auteur si nécessaire
            await IncludeAuteurDetailsAsync(message);


            return message;
        }
        // utilise par la methode 
        public async Task<bool> UpdateMessageAsync(int messageId, string nouveauContenu)
        {
            // Récupérer le message à mettre à jour
            var message = await _messageRepository.GetByIdAsync(messageId);

            // Vérifier si le message existe et si l'utilisateur est l'auteur du message
            if (message != null)
            {
                // Mettre à jour le contenu du message
                message.ContenuMessage = nouveauContenu;

                // Appeler la méthode de mise à jour du repository
                await _messageRepository.UpdateAsync(message);
                await _hubContext.Clients
                      .Group(message.Discussionid.ToString())
                      .SendAsync("NewMessage", message);

                return true; // Mise à jour réussie
            }

            return false; // Message non trouvé ou utilisateur non autorisé
        }



        public async Task AddMultipleMessagesAsync(IEnumerable<Message> messages)
        {
            await _messageRepository.AddRangeAsync(messages);

            // Broadcast a message to all connected clients in each discussion group
            foreach (var message in messages)
            {
                await _hubContext.Clients
                  .Group(message.Discussionid.ToString())
                  .SendAsync("NewMessage", message);
            }
        }

        public async Task RemoveMessage(int messageId, string Contenu, DateTime date, String pseudo, String chemain)
        {
            // Récupérer le message à mettre à jour
            var message = await _messageRepository.GetByIdAsync(messageId);
            await _messageRepository.RemoveAsyn(message);
            await _hubContext.Clients
                   .Group(message.Discussionid.ToString())
                   .SendAsync("NewMessage", message);
        }



        public async Task<IEnumerable<Message>> GetMessagesWithAvatarAsync()
        {
            var messages = await _messageRepository.GetAllAsync();

            foreach (var message in messages)
            {
                var utilisateur = await _utilisateurRepository.GetByIdAsync(message.AuteurId);
                message.Auteur.Cheminavatar = utilisateur.Cheminavatar;
            }

            return messages;
        }

        public async Task<int> HandleUserLoggedInAsync(int userId)
        {
            var unreadMessages = new List<Message>();

            // Récupérer le rôle de l'utilisateur
            var userRoles = await _utilisateurService.GetRolesForUserAsync(userId);

            // Assurez-vous que la liste des rôles n'est pas null
            if (userRoles != null)
            {
                switch (userRoles.FirstOrDefault())
                {
                    case "Administrateur":
                        // Si l'utilisateur est un administrateur, récupère tous les messages non lus
                        unreadMessages = await _messageRepository
                            .Find(m => m.Lu == false)
                            .ToListAsync();
                        break;

                    case "Manager":
                        // Si l'utilisateur est un manager, récupère tous les messages non lus liés à ses discussions
                        var discussionsCrees = await _descussionService.GetDiscussionsCreerParUtilisateurAsync(userId);

                        if (discussionsCrees != null)
                        {
                            foreach (var discussion in discussionsCrees)
                            {
                                var messagesNonLus = await _messageRepository
                                    .Find(m => m.Discussionid == discussion.DiscussionId && m.Lu == false)
                                    .ToListAsync();

                                unreadMessages.AddRange(messagesNonLus);
                            }
                        }
                        break;

                    case "SimpleUser":
                        // Si l'utilisateur est un simple utilisateur, récupère tous les messages non lus liés à ses messages
                        var messagesCrees = await GetMessagesCreerParUtilisateurAsync(userId);

                        if (messagesCrees != null)
                        {
                            foreach (var message in messagesCrees)
                            {
                                var messagesNonLus = await _messageRepository
                                    .Find(m => m.Discussionid == message.Discussionid && m.Lu == false)
                                    .ToListAsync();

                                unreadMessages.AddRange(messagesNonLus);
                            }
                        }
                        break;

                    default:
                        // Gérer le cas par défaut (éventuellement lancer une exception)
                        break;
                }
            }

            // Envoie une notification à l'utilisateur concernant les messages non lus
            //await _hubContext.Clients.User(userId.ToString()).SendAsync("NewMessagesReceived", unreadMessages.Count);

            // Retourne le nombre de messages non lus
            return unreadMessages.Count;
        }


        public async Task<List<Message>> GetMessagesCreerParUtilisateurAsync(int utilisateurId)
        {
            var messages = await _messageRepository.GetMessagesCreer(utilisateurId);
            return messages;
        }
        public async Task<IEnumerable<Message>> GetMessagesByIdsAsync(IEnumerable<int> messageIds)
        {
            
            return await _messageRepository.GetByIdsAsync(messageIds);
        }


        public async Task MarkMessagesAsReadAsync(Message message)
        {
            try
            {
                // Mettre à jour l'état Lu à true
                message.Lu = true;

                // Vérifier si le message a été créé il y a plus d'un mois
                if (DateTime.Now - message.DatecréationMessage > TimeSpan.FromDays(30))
                {
                    // Si oui, marquer le message comme archivé
                    message.Archive = true;
                }

                // Mettre à jour le message dans la base de données
                await _messageRepository.UpdateAsync(message);
            }
            catch (Exception ex)
            {
                // Gérer les erreurs
                Console.WriteLine(ex.ToString());
                // Vous pouvez choisir de lever une exception ou de gérer autrement les erreurs selon vos besoins
            }
        }

    }
}
