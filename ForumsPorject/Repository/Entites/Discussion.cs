using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace ForumsPorject.Repository.Entites
{
    [Table("discussions")]
    [Index(nameof(Themeid), Name = "IX_discussions_themeid")]
    public partial class Discussion
    {
        public Discussion()
        {
            Messages = new HashSet<Message>();
        }

        [Key]
        [Column("discussion_id")]
        public int DiscussionId { get; set; }

        [Column("titre_discussion")]
        [StringLength(255)]
        public string TitreDiscussion { get; set; } = null!;

        [Column("dateCreation_discussion", TypeName = "date")]
        public DateTime DateCreationDiscussion { get; set; }
        
        [Column("themeid")]
        public int? Themeid { get; set; }

        [Column("utilisateurid")]
        public int? Utilisateurid { get; set; }

        [ForeignKey(nameof(Themeid))]
        [InverseProperty("Discussions")]
        public virtual Theme? Theme { get; set; }

        [ForeignKey(nameof(Utilisateurid))]
        [InverseProperty("Discussions")]
        public virtual Utilisateur? Utilisateur { get; set; }
        [InverseProperty(nameof(Message.Discussion))]
        public virtual ICollection<Message> Messages { get; set; }
    }
}
