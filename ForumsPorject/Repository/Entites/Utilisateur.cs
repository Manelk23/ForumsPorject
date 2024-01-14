using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace ForumsPorject.Repository.Entites
{
    [Table("utilisateurs")]
    public partial class Utilisateur
    {
        public Utilisateur()
        {
            Discussions = new HashSet<Discussion>();
            Messages = new HashSet<Message>();
            AppRoles = new HashSet<AppRole>();
         
        }

        [Key]
        [Column("utilisateur_id")]
        public int UtilisateurId { get; set; }
        [Column("pseudonyme")]
        [StringLength(255)]
        public string Pseudonyme { get; set; } = null!;
        [Column("password")]
        [StringLength(255)]
        public string Password { get; set; } = null!;
        [Column("email")]
        [StringLength(255)]
        public string Email { get; set; } = null!;
        [Column("inscrit")]
        public bool Inscrit { get; set; }
        [Column("valid")]
        public bool Valid { get; set; }
        [Column("cheminavatar")]
        [StringLength(255)]
        public string Cheminavatar { get; set; }
        [Column("signature")]
        [StringLength(255)]
        public string? Signature { get; set; }
        [Column("actif")]
        public bool? Actif { get; set; }

        [InverseProperty(nameof(Discussion.Utilisateur))]
        public virtual ICollection<Discussion> Discussions { get; set; }
        [InverseProperty(nameof(Message.Auteur))]
        public virtual ICollection<Message> Messages { get; set; }

        [ForeignKey("UtilisateurId")]
        [InverseProperty(nameof(AppRole.Utilisateurs))]
        public virtual ICollection<AppRole> AppRoles { get; set; }
        public virtual ICollection<UtilisateurRole> UtilisateurRoles { get; set; }

    }
}
