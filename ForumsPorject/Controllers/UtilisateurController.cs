
using Microsoft.AspNetCore.Mvc;
using ForumsPorject.Models;
using ForumsPorject.Repository.ClassesRepository;
using ForumsPorject.Repository.Entites;
using ForumsPorject.Services;
using System.Drawing;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using ForumsProject.Services;

namespace ForumsPorject.Controllers
{
    public class UtilisateurController : Controller
    {
        private readonly UtilisateurService _utilisateurService;
        private readonly JwtService _jwtService;
        private readonly MessageService _messageService;
        private readonly ILogger<UtilisateurController> _logger;
        private readonly UtilisateurRoleService _utilisateurRoleService ;

        public UtilisateurController(UtilisateurService utilisateurService, MessageService messageService,
            ILogger<UtilisateurController> logger, JwtService jwtService, UtilisateurRoleService utilisateurRoleService)
        {
            _utilisateurService = utilisateurService;
            _messageService = messageService;
            _logger = logger;
            _jwtService = jwtService;
            _utilisateurRoleService = utilisateurRoleService;
        }

        [HttpGet] // Utiliser HttpGet pour afficher le formulaire
        public IActionResult RegisterUtilisateur()
        {
            var model = new InputUtilisateur();
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> RegisterUtilisateur(InputUtilisateur model)
        {
            
                // Vérification d'e-mail unique
                var isEmailUnique = await _utilisateurService.IsEmailUniqueAsync(model.Email);

                if (!isEmailUnique)
                {
                    ModelState.AddModelError(nameof(InputUtilisateur.Email), "Cet e-mail est déjà utilisé.");
                    return View(model);
                }

                // Utilisez le service pour créer un utilisateur
                var utilisateur = await _utilisateurService.CreateUtilisateurAsync(
                    model.Pseudonyme, model.Email, model.Cheminavatar, model.Signature, model.Password
                );

                // Vérifiez si l'utilisateur a été créé avec succès
                if (utilisateur != null)
                {
                // Génère un jeton.
                var roles = await _utilisateurService.GetRolesForUserAsync(utilisateur.UtilisateurId);
                var token = _jwtService.GenerateToken(utilisateur);

                Console.WriteLine(token);

                // Enregistrez le chemin de l'avatar dans le cookie
                Response.Cookies.Append("Cheminavatar", utilisateur.Cheminavatar);
                Console.WriteLine("Chemainavatar");

                // Enregistrez les rôles dans le cookie (utilisez Join pour les concaténer en une seule chaîne)
                var rolesString = string.Join(",", roles);
                Response.Cookies.Append("Roles", rolesString);
                Console.WriteLine("Roles");

                return RedirectToAction("Index", "Home");
            }
                else
                {
                    // Gestion des erreurs si la création de l'utilisateur a échoué
                    ModelState.AddModelError(string.Empty, "Erreur lors de la création de l'utilisateur.");
                }
           

            // Les données du formulaire ne sont pas valides, retournez à la vue avec le modèle pour afficher les erreurs
            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> IsEmailUnique(string email)
        {
            var isUnique = await _utilisateurService.IsEmailUniqueAsync(email);
            return Json(isUnique);
        }

        private string HashPassword(string password)
        {
            return BCrypt.Net.BCrypt.HashPassword(password);
        }
        [HttpPost]
        public async Task<ActionResult> LoginUtilisateur()
        {
            var model = new LoginUtilisateurModel();

            if (await TryUpdateModelAsync(model))
            {
                if (ModelState.IsValid)
                {
                    var utilisateur = await _utilisateurService.Authentifier(model.Email, model.Password);
                    //Console.WriteLine(utilisateur.UtilisateurId);
                    if (utilisateur != null)
                    {
                        // Génère un jeton.
                        var roles = await _utilisateurService.GetRolesForUserAsync(utilisateur.UtilisateurId);
                        var token = _jwtService.GenerateToken(utilisateur);

                        Console.WriteLine(token);

                        // Enregistrez le chemin de l'avatar dans le cookie
                        Response.Cookies.Append("Cheminavatar", utilisateur.Cheminavatar);
                        Console.WriteLine("Chemainavatar");

                        // Enregistrez les rôles dans le cookie (utilisez Join pour les concaténer en une seule chaîne)
                        var rolesString = string.Join(",", roles);
                        Response.Cookies.Append("Roles", rolesString);
                        Console.WriteLine("Roles");

                        return RedirectToAction("Index", "Home");
                    }
                    else
                    {
                        ModelState.AddModelError("", "Données de connexion incorrectes.");
                    }
                }
            }

            // Les données du formulaire ne sont pas valides
            return View(model);
        }

        [HttpPost]
        public IActionResult Logout()
        {
            // Efface le cookie d'authentification
            Response.Cookies.Delete("AccessToken");

            // Redirige vers la page d'accueil ou une autre page de votre choix
            return RedirectToAction("Index", "Home");
        }
        public async Task<IActionResult> Index()
        {

            try
            {

                var utilisateur = await _utilisateurService.GetAllWithRolesAsync();

                if (utilisateur == null)
                {
                    return NotFound(); // ou BadRequest("Invalid Category ID");
                }

                var userModels = new List<InputUtilisateur>();

                foreach (var user in utilisateur)
                {
                    var utilisateurMod = new InputUtilisateur
                    {
                        Id = user.UtilisateurId,
                        Pseudonyme = user.Pseudonyme,
                        Email = user.Email,
                        Cheminavatar = user.Cheminavatar,
                        Signature = user.Pseudonyme,

                    };
                    userModels.Add(utilisateurMod);
                }


                return View(userModels);
            }
            catch (Exception ex)
            {
                // Gérer toute exception imprévue
                // Vous pouvez enregistrer l'exception dans un journal, rediriger vers une page d'erreur, ou prendre une autre action appropriée.

                // Exemple : Rediriger vers une page d'erreur avec le message de l'exception
                ViewData["ErrorMessage"] = ex.Message;
                Console.WriteLine(ex.ToString());
                return RedirectToAction("Error");
            }


        }
    
        
           

        // GET: Utilisateurs1/Edit/5
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            try
            {
                // Récupère le token depuis le cookie
                var accessToken = HttpContext.Request.Cookies["AccessToken"];

                // Vérifie si le token est présent
                if (string.IsNullOrEmpty(accessToken))
                {
                    return NotFound();
                }

                // Decode le token pour récupérer les revendications
                var claimsPrincipal = _jwtService.DecodeToken(accessToken);

                // Récupère l'ID de l'utilisateur depuis les revendications
                var utilisateurId = int.Parse(claimsPrincipal.FindFirst(ClaimTypes.NameIdentifier)?.Value);
                var user = await _utilisateurService.GetUserByIdAsync(id);
                if (id != utilisateurId)
                {
                    return RedirectToAction("Error", new { message = "Vous n'êtes pas autorisé à modifier ce compte." });
                }

                var USERMod = new InputUtilisateur
                {
                    Id = user.UtilisateurId,
                    Pseudonyme = user.Pseudonyme,
                    Password = user.Password,
                    Email = user.Email,
                    //Cheminavatar = user.Cheminavatar,
                    Signature = user.Signature,
                   
                };

                // Passe le modèle à la vue
                return View(USERMod);
            }
            catch (Exception ex)
            {
                // Rediriger vers une page d'erreur avec le message de l'exception
                ViewData["ErrorMessage"] = ex.Message;
                Console.WriteLine(ex.ToString());
                return RedirectToAction("Error");
            }
        }


        // POST: Utilisateurs1/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(InputUtilisateur userMod)
        {
            try
            {
                // Valide le modèle
                if (!ModelState.IsValid)
                {
                    return View(userMod);
                }

                // Effectue la mise à jour du message
                var updateResult = await _utilisateurService.UpdateUserAsync(
                    userMod.Id,
                    userMod.Pseudonyme,                   
                    userMod.Email,
                    userMod.Cheminavatar,
                    userMod.Signature,
                    userMod.Password

                );

                // Redirige vers la page d'index si la mise à jour est réussie
                return RedirectToAction("Index", "Home");
            }
            catch (Exception ex)
            {
                // Rediriger vers une page d'erreur avec le message de l'exception
                ViewData["ErrorMessage"] = ex.Message;
                Console.WriteLine(ex.ToString());
                return RedirectToAction("Error");
            }
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                // Récupère le token depuis le cookie
                var accessToken = HttpContext.Request.Cookies["AccessToken"];

                // Vérifie si le token est présent
                if (string.IsNullOrEmpty(accessToken))
                {
                    return NotFound();
                }

                // Decode le token pour récupérer les revendications
                var claimsPrincipal = _jwtService.DecodeToken(accessToken);

                // Récupère l'ID de l'utilisateur depuis les revendications
                var utilisateurId = int.Parse(claimsPrincipal.FindFirst(ClaimTypes.NameIdentifier)?.Value);

                // Récupère le message selon son id
                var user = await _utilisateurService.GetUserByIdAsync(id);

                // Vérifie si l'utilisateur a le droit de supprimer ce message
                if (utilisateurId != id)
                {
                    // Retourner une vue ou une redirection avec un message d'erreur
                    // en indiquant que l'utilisateur n'est pas autorisé à supprimer ce message.
                    return RedirectToAction("Error","Home", new { message = "Vous n'êtes pas autorisé à supprimer ce Compte." });
                }
               
            if (user == null)
                {
                    return NotFound(); // Le message n'a pas été trouvé
                }

                // Passe le modèle à la vue de confirmation
                var userMod = new InputUtilisateur
                {
                    Id = user.UtilisateurId,
                    Pseudonyme = user.Pseudonyme,
                    Email = user.Email,
                    Cheminavatar= user.Cheminavatar
                     
                };

                return View(userMod);
            }
            catch (EntityNotFoundException)
            {
                return NotFound();
            }
        }

        [HttpPost]
        public async Task<IActionResult> Delete(InputUtilisateur userMod)
        {

            try
            {
                if (!ModelState.IsValid)
                {
                    return View(userMod);
                }
                await _utilisateurRoleService.RemoveRolesByUtilisateurIdAsync(userMod.Id);
                // Supprime l'utilisateur
                await _utilisateurService.DeleteUtilisateurAsync(userMod.Id
                   
                     );
                // Valide le modèle

                return View(userMod);


            }
            catch (EntityNotFoundException)
            {
                return NotFound();
            }

            return RedirectToAction("Index", "Home"); // Redirige vers la page d'accueil après la suppression
        }
        
    }
}
    
