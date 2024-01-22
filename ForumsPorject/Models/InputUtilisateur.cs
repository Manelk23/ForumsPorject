using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace ForumsPorject.Models
{
    public class InputUtilisateur
    {

        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        [Display(Name = "Pseudonyme")]
        public string? Pseudonyme { get; set; }

       
        [Remote(action: "IsEmailUnique", controller: "Utilisateur", ErrorMessage = "Cet e-mail est déjà utilisé.")]
        [Required(ErrorMessage = "Le champ e-mail est requis.")]
        [EmailAddress(ErrorMessage = "Veuillez fournir une adresse e-mail valide.")]
        [Display(Name = "Email")]
        public string? Email { get; set; }

        [Required]
        [StringLength(255)]
        [Display(Name = "Chemin Avatar")]
        public string? Cheminavatar { get; set; }

        [Required]
        [StringLength(255)]
        [Display(Name = "Signature")]
        public string? Signature { get; set; }


        [Required]
        [StringLength(100, ErrorMessage = "Le {0} doit avoir au moins {2} et au maximum {1} caractères.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string? Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [Compare("Password", ErrorMessage = "Les mots de passe ne correspondent pas.")]
        public string? ConfirmPassword { get; set; }

        [Required]
        [StringLength(255)]
        [Display(Name = "Roles")]
        public string? Roles { get; set; }

    }
}
