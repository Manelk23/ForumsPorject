using Microsoft.AspNetCore.SignalR;


namespace ForumsPorject.Services
{
    public class ForumHub : Hub
    {
        public string GetConnectionId() => Context.ConnectionId;

        public async Task EnvoyerNotificationNouveauxMessages(string message)
        {
            // Cette méthode peut être appelée depuis le côté client pour envoyer une notification
            // Elle enverra un message à tous les clients connectés
            await Clients.All.SendAsync("RecevoirNotificationNouveauxMessages", message);
        }
        //public async Task SendNewMessage(Message message)
        //{
        //    // Cette méthode est appelée depuis le service pour envoyer un nouveau message à tous les clients connectés
        //    await Clients.Group(message.Discussionid.ToString()).SendAsync("ReceiveNewMessage", message);
        //}

        public async Task NotifyUnreadMessages(int userId)
        {
            // Cette méthode peut être appelée depuis le service pour notifier un client des messages non lus
            await Clients.User(userId.ToString()).SendAsync("ReceiveUnreadMessagesNotification");
        }

        public async Task JoinDiscussionGroup(int discussionId)
        {
            // Joignez le groupe de discussion correspondant à l'ID de la discussion
            await Groups.AddToGroupAsync(Context.ConnectionId, discussionId.ToString());
        }
    }
}

