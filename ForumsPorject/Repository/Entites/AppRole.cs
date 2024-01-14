using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace ForumsPorject.Repository.Entites
{
    public partial class AppRole
    {
        public AppRole()
        {
            Utilisateurs = new HashSet<Utilisateur>();
           

        }

        [Key]
        public int AppRoleId { get; set; }
        [StringLength(255)]
        public string SimpleRole { get; set; } = null!;
        [StringLength(255)]
        public string? ManagerRole { get; set; }

        [ForeignKey("AppRoleId")]
        [InverseProperty(nameof(Utilisateur.AppRoles))]
        public virtual ICollection<Utilisateur> Utilisateurs { get; set; }
        public virtual ICollection<UtilisateurRole> UtilisateurRoles { get; set; }

    }
}
