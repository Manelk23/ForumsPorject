using Microsoft.AspNetCore.Mvc;
using ForumsPorject.Models;
using ForumsPorject.Services;
using System.Diagnostics;
using ForumsPorject.Repository.ClassesRepository;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.Security.Claims;

namespace ForumsPorject.Controllers
{
    public class ThemeController : Controller
    {
        private readonly ThemeService _themeService;
        private readonly JwtService _jwtService;
        private readonly UtilisateurService _utilisateurService;
        public static int Property;
        public ThemeController(ThemeService themeService, JwtService jwtService, UtilisateurService utilisateurService)
        {
            _themeService = themeService;
            _jwtService = jwtService;
            _utilisateurService = utilisateurService;
        }
        [Route("Theme/Index/{id?}")]
        public async Task<IActionResult> Index(int? id)
        {
            if (!id.HasValue)
            {
                return BadRequest("Theme ID is required");
            }
            Property = id.Value;
            var themes = await _themeService.GetThemesByForumIdAsync(id.Value);

            if (themes == null)
            {
                return NotFound(); // ou BadRequest("Invalid Category ID");
            }
            var themesModels = new List<ThemeModel>();
            foreach (var theme in themes)
            {
                var themeMod = new ThemeModel
                {
                    Id = theme.ThemeId,
                    Name = theme.TitreTheme,
                    Date = theme.DateCreationTheme,
                };
                themesModels.Add(themeMod);
            }
            return View(themesModels);
        }
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
            ViewData["Forumid"] = new SelectList("NomForum");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ThemeModel themeModel)
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

                        // Récupère le pseudonyme et le chemin de l'avatar de l'utilisateur
                        var utilisateur = await _utilisateurService.GetUserByIdAsync(utilisateurId);
                        var rolesCookie = Request.Cookies["Roles"];

                        // Divisez la chaîne des rôles en une liste
                        var rolesList = rolesCookie?.Split(',');

                        // Vérifie si l'utilisateur a le rôle d'administrateur
                        if (rolesList == null || !rolesList.Contains("Administrateur"))
                        {
                            // Si l'utilisateur n'a pas le rôle d'administrateur, retourne une réponse interdite
                            return RedirectToAction("Error", "Home", new { message = "Accés Interdit, vous pouvez céer une discussion ou bien partager des messages. Merci" });
                        }
                        await _themeService.CreateThemeAsync(
                            themeModel.Name,
                            themeModel.Date = DateTime.Now,  
                            Property
                        );
                        // Rechargez la liste des  et renvoyez la vue avec le modèle
                        ViewData["Forumid"] = new SelectList("NomForum");
                        return RedirectToAction("Index", "Theme", new { id = Property });
                    }
                    else
                    {
                        // Gérer le cas où le token n'est pas présent
                        return RedirectToAction("Index", "Home");
                    }

                }
                // Rechargez la liste des  et renvoyez la vue avec le modèle
                ViewData["Forumid"] = new SelectList("NomForum");
                return View(themeModel);
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
                var theme = await _themeService.GetThemeByIdAsync(Id);
                // Crée le modèle de message
                var themeMod = new ThemeModel
                {
                    Id = theme.ThemeId,
                    Name = theme.TitreTheme,
                    Date = theme.DateCreationTheme,
                   
                };

                // Passe le modèle à la vue
                return View(themeMod);
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
        public async Task<IActionResult> Edit(ThemeModel themeMod)
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
                // Valide le modèle
                if (!ModelState.IsValid)
                {
                    return View(themeMod);
                }

                // Effectue la mise à jour du message
                var updateResult = await _themeService.UpdateThemeAsync(
                    themeMod.Id,
                    themeMod.Name
                );

                ViewData["Forumid"] = new SelectList("NomForum");
                return RedirectToAction("Index", "Theme", new { id = Property });
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
                // Récupère le message selon son id
                var theme = await _themeService.GetThemeByIdAsync(id);              
                if (theme == null)
                {
                    return NotFound(); // Le message n'a pas été trouvé
                }
                // Passe le modèle à la vue de confirmation
                var themeMod = new ThemeModel
                {
                    Id = theme.ThemeId,
                    Name = theme.TitreTheme,
                    Date = theme.DateCreationTheme,
                    
                };

                return View(themeMod);
            }
            catch (EntityNotFoundException)
            {
                return NotFound();
            }
        }

        [HttpPost]
        public async Task<IActionResult> Delete(ThemeModel themeMod)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return View(themeMod);
                }
                // Supprime le theme
                await _themeService.RemoveTheme(themeMod.Id,
                     themeMod.Name,
                     themeMod.Date          
                     );
                // Valide le modèle
            }
            catch (EntityNotFoundException)
            {
                return NotFound();
            }

            ViewData["Forumid"] = new SelectList("NomForum");
            return RedirectToAction("Index", "Theme", new { id = Property }); // Redirige vers la page d'accueil après la suppression
        }
    }
}




