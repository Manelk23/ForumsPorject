using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
namespace ForumsPorject.Repository.Entites


{
        public class UtilisateurRole
        {
            [Key]
            [ForeignKey("Utilisateur")]
            public int UtilisateurID { get; set; }

            [Key]
            [ForeignKey("AppRole")]
            public int AppRoleId { get; set; }

            public virtual Utilisateur? Utilisateur { get; set; }

            public virtual AppRole? AppRole { get; set; }
        }
}
