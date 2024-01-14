using System.ComponentModel.DataAnnotations.Schema;

namespace ForumsPorject.Models
{
    public class MessageModel
    {
        
       
        public int Id { get; set; }

       
        public string ContenuMessage { get; set; } = null!;

       
        public DateTime DatecréationMessage { get; set; }


        public string? AuteurPseudonyme { get; set; } // Nouvelle propriété
        public string? AuteurAvatarChemin { get; set; } // Nouvelle propriété


        public bool Lu { get; set; }


        public bool Archive { get; set; }
        public int AuteurId { get; set; }

        public int? Discussionid { get; set; }

       


    }

}

