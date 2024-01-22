using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace ForumsPorject.Repository.Entites
{
    [Table("messages")]
    [Index(nameof(AuteurId), Name = "IX_messages_auteur_id")]
    [Index(nameof(Discussionid), Name = "IX_messages_discussionid")]
    public partial class Message
    {
        [Key]
        [Column("messages_id")]
        public int MessagesId { get; set; }

        [Column("contenu_message")]
        public string ContenuMessage { get; set; } = null!;

        [Column("datecréation_message", TypeName = "date")]
        public DateTime DatecréationMessage { get; set; }


        [Column("lu")]
        public bool Lu { get; set; }

        [Column("archive")]
        public bool Archive { get; set; }

        [Column("auteur_id")]
        public int AuteurId { get; set; }

        [Column("discussionid")]
        public int? Discussionid { get; set; }

        [ForeignKey(nameof(AuteurId))]
        [InverseProperty(nameof(Utilisateur.Messages))]
        public virtual Utilisateur Auteur { get; set; } = null!;

        [ForeignKey(nameof(Discussionid))]
        [InverseProperty("Messages")]
        
        public virtual Discussion Discussion { get; set; } = null!;

        public virtual ICollection<MessageReadState> MessageReadStates { get; set; } = new List<MessageReadState>();
    }
}
