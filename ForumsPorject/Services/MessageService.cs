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

        public MessageService(
            MessageRepository messageRepository,
            UtilisateurRepository utilisateurRepository,
            IHubContext<ForumHub> hubContext
        )
        {
            _messageRepository = messageRepository;
            _utilisateurRepository = utilisateurRepository;
            _hubContext = hubContext;
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
            if (message != null )
            {
                // Mettre à jour le contenu du message
                message.ContenuMessage = nouveauContenu;

                // Appeler la méthode de mise à jour du repository
                _messageRepository.UpdateAsync(message);

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
              await  _hubContext.Clients
                    .Group(message.Discussionid.ToString())
                    .SendAsync("NewMessage", message);
            }
        }

        public void UpdateMessage(Message message)
        {
            _messageRepository.Update(message);
        }

        public void UpdateMultipleMessages(IEnumerable<Message> messages)
        {
            _messageRepository.UpdateRange(messages);
        }

        public void RemoveMessage(Message message)
        {
            _messageRepository.Remove(message);
        }

        public async Task RemoveMessage(int messageId, string Contenu, DateTime date, String pseudo , String chemain)
        {
            // Récupérer le message à mettre à jour
            var message = await _messageRepository.GetByIdAsync(messageId);
            await _messageRepository.RemoveAsyn(message);
            await _hubContext.Clients
                   .Group(message.Discussionid.ToString())
                   .SendAsync("NewMessage", message);
        }

        public void RemoveMultipleMessages(IEnumerable<Message> messages)
        {
            _messageRepository.RemoveRange(messages);
        }

       

        public async Task<IEnumerable<Message>> GetMessagesByUserIdAsync(int userId)
        {
            return await _messageRepository.Find(m => m.AuteurId == userId).ToListAsync();
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

            public async Task HandleUserLoggedInAsync(int userId)
            {
                // Retrieve unread messages for the user
                var unreadMessages = await _messageRepository
                    .Find(m => m.AuteurId == userId && m.Lu == false)
                    .ToListAsync();
            // Send a notification to the user about unread messages
            await _hubContext.Clients.User(userId.ToString()).SendAsync("NewMessagesReceived", unreadMessages.Count);
            }





        public async Task<IEnumerable<Message>> GetAllMessagesAsync()
        {
            return await _messageRepository.GetAllAsync();
        }
    }



    }
