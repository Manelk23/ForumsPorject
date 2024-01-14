
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
//using NuGet.Versioning;

namespace ForumsPorject.Controllers
{
    public class DiscussionsController : Controller
    {
        private readonly DescussionService _descussionService;
        private readonly MessageService _messageService;
        private readonly JwtService _jwtService;
        public static int Property;

        public DiscussionsController(DescussionService descussionService, MessageService messageService, JwtService jwtService)
        {
            _descussionService = descussionService;
            _messageService = messageService;
            _jwtService = jwtService;
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

                        // Appelle le service avec les informations nécessaires
                        await _descussionService.CreateDiscussionAsync(
                            discussionModel.Title,
                            discussionModel.DateCreationDiscussion = DateTime.Now,
                            Property,
                            utilisateurId
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
                ViewData["Themeid"] = new SelectList("NomTheme");
                return View(discussionModel);
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
        
        public async Task<IActionResult> Index(int? id)
        {
            if (!id.HasValue)
            {
               
                return BadRequest("Discussion ID is required");
            }
            Property = id.Value;
            var discussions = await _descussionService.GetDiscussionByThemeIdAsync(id.Value);

            if (discussions == null)
            {
                return NotFound(); // ou BadRequest("Invalid Category ID");
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
       


        private bool DiscussionExists(int id)
        {
            return _descussionService.GetDiscussionByIdAsync(id).Result != null;
        }

        public int getThemId(int?  id)
        {
            return (int)((id != null) ? id : null);

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

                // Récupère le message selon son id 
                var descussion = await _descussionService.GetDiscussionByIdAsync(Id);

                if (utilisateurId != descussion.Utilisateurid)
                {
                    // Retourner une vue ou une redirection avec un message d'erreur
                    // en indiquant que l'utilisateur n'est pas autorisé à modifier ce message.
                    return RedirectToAction("Error", new { message = "Vous n'êtes pas autorisé à modifier cette discussion." });
                }

                // Crée le modèle de message
                var discussionMod = new DiscussionModel
                {
                    Id = descussion.DiscussionId,
                    Title = descussion.TitreDiscussion,
                    DateCreationDiscussion = descussion.DateCreationDiscussion,
                    ThemeId= descussion.Themeid
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
                var discussion = await _descussionService.GetDiscussionByIdAsync(id);

                // Vérifie si l'utilisateur a le droit de supprimer ce message
                if (utilisateurId != discussion.Utilisateurid)
                {
                    // Retourner une vue ou une redirection avec un message d'erreur
                    // en indiquant que l'utilisateur n'est pas autorisé à supprimer cette discussion.
                    return RedirectToAction("Error", new { message = "Vous n'êtes pas autorisé à supprimer cette discussion." });
                }

                if (discussion == null)
                {
                    return NotFound(); // Le message n'a pas été trouvé
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

            return RedirectToAction("Index", "Home"); // Redirige vers la page d'accueil après la suppression
        }
    }
}
   





