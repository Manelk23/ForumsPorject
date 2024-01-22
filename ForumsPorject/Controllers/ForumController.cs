using Microsoft.AspNetCore.Mvc;
using ForumsPorject.Models;
using System.Diagnostics;
using ForumsPorject.Services;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.Security.Claims;
using ForumsPorject.Repository.ClassesRepository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.IdentityModel.Tokens;

namespace ForumProject.Controllers
{
    public class ForumController : Controller
    {
        private readonly ForumService _service;
        private readonly JwtService _jwtService;
        private readonly UtilisateurService _utilisateurService;
        public static int Property;
        public ForumController(ForumService service, JwtService jwtService, UtilisateurService utilisateurService)
        {
            _service = service;
            _jwtService = jwtService;
            _utilisateurService = utilisateurService;
        }

        public async Task<IActionResult> Index(int? id)
        {
            var accessToken = HttpContext.Request.Cookies["AccessToken"];
            if (accessToken == null)
            {
                TempData["ErrorMessage"] = "Vous devez vous connecter pour accéder à cette page.";
                return RedirectToAction("Error", "Home", new { message = TempData["ErrorMessage"] });
            }
            // Decode le token pour récupérer les revendications
            var claimsPrincipal = _jwtService.DecodeToken(accessToken);
            

            // Récupère l'ID de l'utilisateur depuis les revendications
            var utilisateurId = int.Parse(claimsPrincipal.FindFirst(ClaimTypes.NameIdentifier)?.Value);
           
            

            if (!id.HasValue)
            {
                return BadRequest("Category ID is required");
            }

            Property = id.Value;
            var forumes = await _service.GetForumsByCategoryIdAsync(id.Value);

            if (forumes == null)
            {
                return NotFound(); // ou BadRequest("Invalid Category ID");
            }

            var forumsModels = new List<ForumModel>();

            foreach (var forum in forumes)
            {
                var forumesMod = new ForumModel
                {
                    Id = forum.ForumId,
                    Name = forum.TitreForum,
                    dateCreation = forum.DateCreationForum,
                    Discription = forum.DiscriptionForum,
                };
                forumsModels.Add(forumesMod);
            }

            return View(forumsModels);
        }

        // GET: Forum/Create
        public IActionResult Create()
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
                if (rolesList == null || !rolesList.Contains("Administrateur"))
                {
                // Si l'utilisateur n'a pas le rôle d'administrateur, retourne une réponse interdite
                return RedirectToAction("Error", "Home", new { message = "Accés Interdit, vous pouvez céer une discussion ou bien partager des messages. Merci" });
            }
                ViewData["Ctegirieid"] = new SelectList("NomCtegorie");
            return View();
        }
        [HttpPost]

        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ForumModel forumModel)
        {
            try
            {
  

                if (ModelState.IsValid)
                {
                    // Récupère le token depuis le cookie
                    var accessToken = HttpContext.Request.Cookies["AccessToken"];
                    // Vérifie si le token est présent
                    if (!string.IsNullOrEmpty(accessToken))
                    {
                        // Decode le token pour récupérer les revendications
                        var claimsPrincipal = _jwtService.DecodeToken(accessToken);

                        // Récupère l'ID de l'utilisateur depuis les revendications
                        var utilisateurId = int.Parse(claimsPrincipal.FindFirst(ClaimTypes.NameIdentifier)?.Value);
                        // Récupère les rôles à partir du cookie "Roles"
                        var rolesCookie = Request.Cookies["Roles"];

                        // Divisez la chaîne des rôles en une liste
                        var rolesList = rolesCookie?.Split(',');

                        // Vérifie si l'utilisateur a le rôle d'administrateur
                        if (rolesList == null || !rolesList.Contains("Administrateur"))
                        {
                            // Si l'utilisateur n'a pas le rôle d'administrateur, retourne une réponse interdite
                            return RedirectToAction("Error", "Home", new { message = "Accés Interdit, vous pouvez céer une discussion ou bien partager des messages. Merci" });
                        }
                        // Appelle le service avec les informations nécessaires
                        await _service.CreateForumAsync(
                            forumModel.Name,
                            forumModel.dateCreation = DateTime.Now,
                            forumModel.Discription,
                            Property
                            
                        );

                        return RedirectToAction("Index", "Home");
                    }
                    else
                    {
                        // Gérer le cas où le token n'est pas présent
                        return RedirectToAction("Index", "Home");
                    }
                }

                // Rechargez la liste des thèmes et renvoyez la vue avec le modèle
                ViewData["Ctegorieid"] = new SelectList("NomCategorie");
                return View(forumModel);
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
        public async Task<IActionResult> Edit(int Id)
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
                if (rolesList == null || !rolesList.Contains("Administrateur"))
                {
                    // Si l'utilisateur n'a pas le rôle d'administrateur, retourne une réponse interdite
                    return RedirectToAction("Error", "Home", new { message = "Accés Interdit, vous pouvez céer une discussion ou bien partager des messages. Merci" });
                }
                // Récupère le message selon son id 
                var forum = await _service.GetForumByIdAsync(Id);
                // Crée le modèle de message
                var forumMod = new ForumModel
                {
                    Id = forum.ForumId,
                    Name = forum.TitreForum,
                    dateCreation = forum.DateCreationForum,
                    Discription = forum.DiscriptionForum,
                };

                // Passe le modèle à la vue
                return View(forumMod);
            }
            catch (Exception ex)
            {
                // Gérer toute exception imprévue
                ViewData["ErrorMessage"] = ex.Message;
                Console.WriteLine(ex.ToString());
                return RedirectToAction("Error");
            }
        }

        [HttpPost]
        public async Task<IActionResult> Edit(ForumModel forumMod)
        {
            try
            { // Récupère le token depuis le cookie
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
                if (rolesList == null || !rolesList.Contains("Administrateur"))
                {
                    // Si l'utilisateur n'a pas le rôle d'administrateur, retourne une réponse interdite
                    return RedirectToAction("Error", "Home", new { message = "Accés Interdit, vous pouvez céer une discussion ou bien partager des messages. Merci" });
                }
                // Valide le modèle
                if (!ModelState.IsValid)
                {
                    return View(forumMod);
                }
                // Effectue la mise à jour du message
                var updateResult = await _service.UpdateForumAsync(
                    forumMod.Id,
                    forumMod.Name,
                    forumMod.dateCreation,
                    forumMod.Discription

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
                if (rolesList == null || !rolesList.Contains("Administrateur"))
                {
                    // Si l'utilisateur n'a pas le rôle d'administrateur, retourne une réponse interdite
                    return RedirectToAction("Error", "Home", new { message = "Accés Interdit, vous pouvez céer une discussion ou bien partager des messages. Merci" });

                }
                var forum = await _service.GetForumByIdAsync(id);
                if (forum == null)
                {
                    return NotFound(); // Le message n'a pas été trouvé
                }
                // Passe le modèle à la vue de confirmation
                var forumMod = new ForumModel
                {
                    Id = forum.ForumId,
                    Name = forum.TitreForum,
                    dateCreation = forum.DateCreationForum,
                    Discription = forum.DiscriptionForum,
                    CategorieId = forum.Categorieid
                };
                return View(forumMod);
            }
            catch (EntityNotFoundException)
            {
                return NotFound();
            }
        }

        [HttpPost]

        public async Task<IActionResult> Delete(ForumModel forumMod)

        {
            try { 
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
            if (rolesList == null || !rolesList.Contains("Administrateur"))
            {
                    // Si l'utilisateur n'a pas le rôle d'administrateur, retourne une réponse interdite
                    return RedirectToAction("Error", "Home", new { message = "Accés Interdit, vous pouvez céer une discussion ou bien partager des messages. Merci" });
                }
            if (!ModelState.IsValid)
                {
                    return View(forumMod);
                }
                // Supprime le message
                await _service.RemoveForum(forumMod.Id,
                     forumMod.Name,
                     forumMod.dateCreation,
                     forumMod.Discription
                     );
                // Valide le modèle
            }
            catch (EntityNotFoundException)
            {
                return NotFound();
            }
            return RedirectToAction("Index", "Home"); // Redirige vers la page d'accueil après la suppression
        }
    }
}