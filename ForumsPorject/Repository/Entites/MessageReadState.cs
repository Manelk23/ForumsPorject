using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace ForumsPorject.Repository.Entites
{ 
    public class MessageReadState
    {
        public int MessageReadStateId { get; set; }
        public int UtilisateurId { get; set; }
        public int MessageId { get; set; }
        public bool IsRead { get; set; }
        public DateTime DateRead { get; set; }

        public virtual Utilisateur Utilisateur { get; set; }
        public virtual Message Message { get; set; }
      
       
    }
}
