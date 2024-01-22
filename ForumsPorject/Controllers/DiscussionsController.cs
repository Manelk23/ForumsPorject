
using System;
using System.Diagnostics;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
//using Microsoft.CodeAnalysis.CSharp.Syntax;
using ForumsPorject.Models;
using ForumsPorject.Repository.Entites;
using ForumsPorject.Services;
using ForumsPorject.Repository.ClassesRepository;
using Microsoft.CodeAnalysis.CSharp.Syntax;
//using NuGet.Versioning;

namespace ForumsPorject.Controllers
{
    public class DiscussionsController : Controller
    {
        private readonly DescussionService _descussionService;
        private readonly MessageService _messageService;
        private readonly JwtService _jwtService;
        private readonly UtilisateurService _utilisateurService;
        public static int Property;

        public DiscussionsController(DescussionService descussionService, MessageService messageService, 
            JwtService jwtService, UtilisateurService utilisateurService)
        {
            _descussionService = descussionService;
            _messageService = messageService;
            _jwtService = jwtService;
            _utilisateurService = utilisateurService;
        }

        [Route("Discussion/Index/{id?}")]
        public async Task<IActionResult> Index(int? id)
        {
            if (!id.HasValue)
            {
                return RedirectToAction("Error", "Home", new { message = "Discussion ID est incorrecte" });
            }
            Property = id.Value;
            var discussions = await _descussionService.GetDiscussionByThemeIdAsync(id.Value);

            if (discussions == null)
            {
                return RedirectToAction("Error", "Home", new { message = "Theme ID est incorrecte" });
            }

            var DiscussionModels = new List<DiscussionModel>();

            foreach (var Discussion in discussions)
            {
                var discussioMod = new DiscussionModel
                {
                    Id = Discussion.DiscussionId,
                    Title = Discussion.TitreDiscussion,
                    DateCreationDiscussion = Discussion.DateCreationDiscussion,
                    ThemeId = Discussion.Themeid,
                };
                DiscussionModels.Add(discussioMod);
            }
            return View(DiscussionModels);
        }

        // GET: Discussions/Create
        public IActionResult Create()
        {
            ViewData["Themeid"] = new SelectList("NomTheme");
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(DiscussionModel discussionModel)
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
                       
                            await _descussionService.CreateDiscussionAsync(
                                discussionModel.Title,
                                discussionModel.DateCreationDiscussion = DateTime.Now,
                                Property,
                                utilisateurId
                            );
                            ViewData["Themeid"] = new SelectList("NomTheme");
                            return RedirectToAction("Index", "Discussion", new { id = Property });
                       
                    }
                    else
                    {
                        // Gérer le cas où le token n'est pas présent
                        return RedirectToAction("Error", "Home", new { message = "Vous devez créer un compte ou vous connecter." });
                    }
                }

                // Rechargez la liste des thèmes et renvoyez la vue avec le modèle
                ViewData["Themeid"] = new SelectList("NomTheme");
                return RedirectToAction("Index", "Discussion", new { id = Property });
            }
            catch (Exception ex)
            {
               
                // Exemple : Rediriger vers une page d'erreur avec le message de l'exception
                ViewData["ErrorMessage"] = ex.Message;
                Console.WriteLine(ex.ToString());
                return RedirectToAction("Error", "Home", new { message = "Impossible de créer une discussion." });
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

                // Récupère le pseudonyme et le chemin de l'avatar de l'utilisateur
                var utilisateur = await _utilisateurService.GetUserByIdAsync(utilisateurId);
                var rolesCookie = Request.Cookies["Roles"];

                // Divisez la chaîne des rôles en une liste
                var rolesList = rolesCookie?.Split(',');

                // Récupère la discussion par son ID
                var discussion = await _descussionService.GetDiscussionByIdAsync(Id);

                // Vérifie si l'utilisateur a le rôle d'administrateur ou s'il est l'auteur de la discussion
                if (rolesList == null || (!rolesList.Contains("Administrateur") && utilisateurId != discussion.Utilisateurid))
                {
                    return RedirectToAction("Error", "Home", new { message = "Vous n'êtes pas autorisé à modifier cette discussion." });
                }

                // Crée le modèle de discussion
                var discussionMod = new DiscussionModel
                {
                    Id = discussion.DiscussionId,
                    Title = discussion.TitreDiscussion,
                    DateCreationDiscussion = discussion.DateCreationDiscussion,
                    ThemeId = discussion.Themeid
                };

                // Passe le modèle à la vue
                return View(discussionMod);
            }
            catch (Exception ex)
            {
                // Rediriger vers une page d'erreur avec le message de l'exception
                ViewData["ErrorMessage"] = ex.Message;
                Console.WriteLine(ex.ToString());
                return RedirectToAction("Error");
            }
        }

        [HttpPost]
        public async Task<IActionResult> Edit(DiscussionModel discussionMod)
        {
            try
            {
                // Valide le modèle
                if (!ModelState.IsValid)
                {
                    return View(discussionMod);
                }

                // Effectue la mise à jour du message
                var updateResult = await _descussionService.UpdateDiscussionAsync(
                    discussionMod.Id,
                    discussionMod.Title
                   
                );

                // Redirige vers la page d'index si la mise à jour est réussie           
                return RedirectToAction("Index", "Discussion", new { id = Property });
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

                var rolesCookie = Request.Cookies["Roles"];

                // Divisez la chaîne des rôles en une liste
                var rolesList = rolesCookie?.Split(',');

                // Récupère la discussion par son ID
                var discussion = await _descussionService.GetDiscussionByIdAsync(id);

                // Vérifie si l'utilisateur a le rôle d'administrateur ou s'il est l'auteur de la discussion
                if (rolesList == null || (!rolesList.Contains("Administrateur") && utilisateurId != discussion.Utilisateurid))
                {
                    return RedirectToAction("Error", "Home", new { message = "Vous n'êtes pas autorisé à supprimer cette discussion." });
                }
                // Passe le modèle à la vue de confirmation
                var discussionMod = new DiscussionModel
                {
                    Id = discussion.DiscussionId,
                    Title = discussion.TitreDiscussion,
                    DateCreationDiscussion = discussion.DateCreationDiscussion,
                    ThemeId = discussion.Themeid
                };

                return View(discussionMod);
            }
            catch (EntityNotFoundException)
            {
                return NotFound();
            }
        }
        [HttpPost]
        public async Task<IActionResult> Delete(DiscussionModel discussionMod)
        {

            try
            {
                if (!ModelState.IsValid)
                {
                    return View(discussionMod);
                }

                // Supprime le message
                await _descussionService.RemoveDiscussion(discussionMod.Id,
                     discussionMod.Title,
                     discussionMod.DateCreationDiscussion                  
                     );
                // Valide le modèle

            }
            catch (EntityNotFoundException)
            {
                return NotFound();
            }
         
            return RedirectToAction("Index", "Discussion", new { id = Property }); // Redirige vers la page d'accueil après la suppression
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var discussion = await _descussionService.GetDiscussionByIdAsync(id.Value);

            if (discussion == null)
            {
                return NotFound();
            }

            return View(discussion);
        }
    }
}
   





