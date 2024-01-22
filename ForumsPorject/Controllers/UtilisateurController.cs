
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
using System.Net.Mail;
using System.Net;

namespace ForumsPorject.Controllers
{
    public class UtilisateurController : Controller
    {
        private readonly UtilisateurService _utilisateurService;
        private readonly JwtService _jwtService;
        private readonly MessageService _messageService;
        private readonly ILogger<UtilisateurController> _logger;
        private readonly UtilisateurRoleService _utilisateurRoleService ;
        private readonly DescussionService _descussionService;

        public UtilisateurController(UtilisateurService utilisateurService, MessageService messageService,
            ILogger<UtilisateurController> logger, JwtService jwtService, UtilisateurRoleService utilisateurRoleService, DescussionService descussionService)
        {
            _utilisateurService = utilisateurService;
            _messageService = messageService;
            _logger = logger;
            _jwtService = jwtService;
            _utilisateurRoleService = utilisateurRoleService;
            _descussionService = descussionService;
        }
        [HttpGet]
        public IActionResult RegisterUtilisateur()
        {
            var model = new InputUtilisateur();
            return View(model);
        }


        [HttpPost]
        public async Task<IActionResult> RegisterUtilisateur(InputUtilisateur model)
         {
           
            try
            {
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
                                  // Envoie de l'e-mail de confirmation
                             //await SendConfirmationEmail(model.Email);

                             return RedirectToAction("Index", "Home");
                        }
                        else
                        {

                          return View(model);
                        }
                    
            }
            catch (Exception ex)
            {
                // Gérez l'exception ici (enregistrez dans les journaux, affichez un message d'erreur, etc.)
                ModelState.AddModelError(string.Empty, "Une erreur inattendue s'est produite lors de la création de l'utilisateur.");
            }

            // Les données du formulaire ne sont pas valides ou une erreur s'est produite, retournez à la vue avec le modèle pour afficher les erreurs
            return View(model);
        }


        private async Task SendConfirmationEmail(string userEmail)
        {
            // Adresse e-mail de l'expéditeur
            string senderEmail = "nasr.manel.m@gmail.com"; // Remplacez par votre adresse Gmail

            // Créez un objet MailMessage
            var mail = new MailMessage(senderEmail, userEmail);

            // Sujet et corps du message
            mail.Subject = "Confirmation de votre compte";
            mail.Body = "Merci de vous être inscrit sur notre site. Veuillez confirmer votre compte en cliquant sur le lien suivant.";

            // Configuration du client SMTP pour Gmail
            using (var client = new SmtpClient("smtp.gmail.com"))
            {
                client.Port = 587; // Utilisez le port TLS
                client.Credentials = new NetworkCredential("nasr.manel.m@gmail.com", "mannoula2316");
                client.EnableSsl = true;

                try
                {
                    // Envoi de l'e-mail
                    await client.SendMailAsync(mail);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Erreur lors de l'envoi de l'e-mail : {ex.Message}");
                    throw; // Permet de propager l'exception pour une meilleure traçabilité
                }
            }
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
                    await IsEmailUnique(model.Email);
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
                        await ApresConnexion(utilisateur.UtilisateurId);

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

        [HttpGet]
        public IActionResult Logout()
        {
            // Efface le cookie d'authentification
            Response.Cookies.Delete("AccessToken");
            // Efface le cookie du chemin de l'avatar
            Response.Cookies.Delete("Cheminavatar");

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
        [HttpGet]
        public async Task<IActionResult> ApresConnexion(int id)
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

                    // Appeler la méthode HandleUserLoggedInAsync du service pour envoyer des notifications
                    var unreadMessagesCount = await _messageService.HandleUserLoggedInAsync(utilisateurId);
                    if (unreadMessagesCount == null)
                    {
                        return RedirectToAction("Index", "Home");
                    }
                else
                {
                    // Enregistrez le nombre de messages non lus dans le cookie
                    Response.Cookies.Append("UnreadMessagesCount", unreadMessagesCount.ToString());

                    return RedirectToAction("Index", "Home");
                }
            }
            catch (Exception ex)
            {
                // Gérer les erreurs
                return RedirectToAction("Error", "Home", new { message = ex.Message });
            }
        }
        [HttpPost]
        public async Task<IActionResult> ConsultationMessage(int id)
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
                var message = await _messageService.GetMessageByIdAsync(id);
                var messageMod = new MessageModel();
                if (message == null)
                {
                    return NotFound(); // Le message n'a pas été trouvé
                }
                return View(messageMod);
            }

            catch (Exception ex)
            {
                // Gérer les erreurs
                return RedirectToAction("Error", "Home", new { message = ex.Message });
            }
        }

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

                // Récupère les rôles à partir du cookie "Roles"
                var rolesCookie = Request.Cookies["Roles"];

                // Divisez la chaîne des rôles en une liste
                var rolesList = rolesCookie?.Split(',');

                // Vérifie si l'utilisateur a le rôle d'administrateur
                var isAdmin = rolesList != null && rolesList.Contains("Administrateur");

                // Vérifie si l'utilisateur a le droit de modifier
                var canModify = isAdmin || id == utilisateurId;

                if (!canModify)
                {
                    // Retourne une vue avec le message d'erreur
                    return RedirectToAction("Error", "Home", new { message = "Vous n'êtes pas autorisé à modifier ce Compte." });
                }

                // L'utilisateur a le droit de modifier, récupère les informations de l'utilisateur
                var user = await _utilisateurService.GetUserByIdAsync(id);
                if (user == null)
                {
                    return NotFound();
                }

                var userMod = new InputUtilisateur
                {
                    Id = user.UtilisateurId,
                    Pseudonyme = user.Pseudonyme,
                    Password = user.Password,
                    Email = user.Email,
                    Signature = user.Signature,
                };

                // Passe le modèle à la vue
                return View(userMod);
            }
            catch (EntityNotFoundException)
            {
                return NotFound();
            }
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(InputUtilisateur userMod)
        {
            try
            {
            
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

                // Récupère les rôles à partir du cookie "Roles"
                var rolesCookie = Request.Cookies["Roles"];

                // Divisez la chaîne des rôles en une liste
                var rolesList = rolesCookie?.Split(',');

                // Vérifie si l'utilisateur a le rôle d'administrateur
                var isAdmin = rolesList != null && rolesList.Contains("Administrateur");

                // Vérifie si l'utilisateur a le droit de modifier
                var canModify = isAdmin || id == utilisateurId;

                if (!canModify)
                {
                    // Retourne une vue avec le message d'erreur
                    return RedirectToAction("Error", "Home", new { message = "Vous n'êtes pas autorisé à supprimer ce Compte." });
                }

                // Récupère le message selon son id
                var user = await _utilisateurService.GetUserByIdAsync(id);

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
                    Cheminavatar = user.Cheminavatar
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
                //if (!ModelState.IsValid)
                //{
                //    return View(userMod);
                //}
                await _utilisateurRoleService.RemoveRolesByUtilisateurIdAsync(userMod.Id);
                // Supprime l'utilisateur
                await _utilisateurService.DeleteUtilisateurAsync(userMod.Id
                   
                     );
                // Valide le modèle

                return RedirectToAction("Index", "Home");

            }
            catch (EntityNotFoundException)
            {
                return NotFound();
            }

            
        }
        
    }
}
    
