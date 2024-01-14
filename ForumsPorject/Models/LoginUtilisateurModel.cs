using System.ComponentModel.DataAnnotations;

namespace ForumsPorject.Models
{
	public class LoginUtilisateurModel
	{
        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string? Email { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "Le {0} doit avoir au moins {2} et au maximum {1} caractères.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string? Password { get; set; }

    }
}
