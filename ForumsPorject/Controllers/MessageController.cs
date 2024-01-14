using Microsoft.AspNetCore.Mvc;
using ForumsPorject.Repository.ClassesRepository;
using ForumsPorject.Repository.Entites;
using ForumsPorject.Services;
using ForumsPorject.Models;
using System.Security.Claims;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace ForumsPorject.Controllers
{
    public class MessageController : Controller
    {
        private readonly MessageService _messageService;
        private readonly JwtService _jwtService;
        private readonly UtilisateurService _utilisateurService;
        private readonly DescussionService _descussionService;
        public static int Property;
        public MessageController(MessageService messageService, UtilisateurService utilisateurService, DescussionService descussionService, JwtService jwtService)
        {
            _messageService = messageService;
            _jwtService = jwtService;
            _utilisateurService = utilisateurService;
            _descussionService = descussionService;
        }


        public async Task<IActionResult> Index(int? id)
        {
            if (!id.HasValue)
            {

                return BadRequest("Message ID is required");
            }
            Property = id.Value;
            var messages = await _messageService.GetMessagesByDiscussionAsync(id.Value);

            if (messages == null)
            {
                return NotFound(); // ou BadRequest("Invalid Category ID");
            }

            var MessModels = new List<MessageModel>();

            foreach (var message in messages)
            {
                var messageMod = new MessageModel
                {
                    Id = message.MessagesId,
                    ContenuMessage = message.ContenuMessage,
                    DatecréationMessage = message.DatecréationMessage,
                    AuteurAvatarChemin = message.Auteur.Cheminavatar,
                    AuteurPseudonyme = message.Auteur.Pseudonyme,
                };
                       MessModels.Add(messageMod);
            }


            return View(MessModels);
        }


        public IActionResult Create()
        {
            ViewData["Discussionid"] = new SelectList("NomDiscussion");
            return View();


        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(MessageModel messageModel)
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
                        var pseudonyme = utilisateur.Pseudonyme;
                        var avatarChemin = utilisateur.Cheminavatar;
                        
                        await _messageService.CreateMessageAsync(
                            messageModel.ContenuMessage,
                            messageModel.DatecréationMessage = DateTime.Now,
                            messageModel.Lu= false,
                            messageModel.Archive=false,
                            messageModel.AuteurId =utilisateurId,
                            messageModel.Discussionid= Property
                           
                        );

                        messageModel.AuteurPseudonyme = pseudonyme;
                        messageModel.AuteurAvatarChemin = avatarChemin;

                        // Rechargez la liste des  et renvoyez la vue avec le modèle
                        ViewData["Discussionid"] = new SelectList("NomDiscussion");
                        return RedirectToAction("Index", "Message"); ;
                    }
                    else
                    {
                        // Gérer le cas où le token n'est pas présent
                        return RedirectToAction("Index", "Home");
                    }

                }
                // Rechargez la liste des  et renvoyez la vue avec le modèle
                ViewData["Discussionid"] = new SelectList("NomDiscussion");
                return View(messageModel);
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

                // Récupère le message selon son id 
                var message = await _messageService.GetMessageByIdAsync(Id);

                if (utilisateurId != message.AuteurId)
                {
                    // Retourner une vue ou une redirection avec un message d'erreur
                    // en indiquant que l'utilisateur n'est pas autorisé à modifier ce message.
                    return RedirectToAction("Error", new { message = "Vous n'êtes pas autorisé à modifier ce message." });
                }

                // Crée le modèle de message
                var messageMod = new MessageModel
                {
                    Id = message.MessagesId,
                    ContenuMessage = message.ContenuMessage,
                    DatecréationMessage = message.DatecréationMessage,
                    AuteurAvatarChemin = message.Auteur.Cheminavatar,
                    AuteurPseudonyme = message.Auteur.Pseudonyme,
                };

                // Passe le modèle à la vue
                return View(messageMod);
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
        public async Task<IActionResult> Edit(MessageModel messageMod)
        {
            try
            {
                // Valide le modèle
                if (!ModelState.IsValid)
                {
                    return View(messageMod);
                }

                // Effectue la mise à jour du message
                var updateResult = await _messageService.UpdateMessageAsync(
                    messageMod.Id,
                    messageMod.ContenuMessage
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
                var message = await _messageService.GetMessageByIdAsync(id);

                // Vérifie si l'utilisateur a le droit de supprimer ce message
                if (utilisateurId != message.AuteurId)
                {
                    // Retourner une vue ou une redirection avec un message d'erreur
                    // en indiquant que l'utilisateur n'est pas autorisé à supprimer ce message.
                    return RedirectToAction("Error", new { message = "Vous n'êtes pas autorisé à supprimer ce message." });
                }

                if (message == null)
                {
                    return NotFound(); // Le message n'a pas été trouvé
                }

                // Passe le modèle à la vue de confirmation
                var messageMod = new MessageModel
                {
                    Id = message.MessagesId,
                    ContenuMessage = message.ContenuMessage,
                    DatecréationMessage = message.DatecréationMessage,
                    AuteurAvatarChemin = message.Auteur.Cheminavatar,
                    AuteurPseudonyme = message.Auteur.Pseudonyme,
                };

                return View(messageMod);
            }
            catch (EntityNotFoundException)
            {
                return NotFound();
            }
        }

        [HttpPost]
        public async Task<IActionResult> Delete(MessageModel messageMod)
        {

            try
            {
                if (!ModelState.IsValid)
                {
                    return View(messageMod);
                }

                // Supprime le message
                await _messageService.RemoveMessage(messageMod.Id,
                     messageMod.ContenuMessage,
                     messageMod.DatecréationMessage,
                     messageMod.AuteurAvatarChemin,
                     messageMod.AuteurPseudonyme
                     );
                // Valide le modèle




            }
            catch (EntityNotFoundException)
            {
                return NotFound();
            }

            return RedirectToAction("Index", "Home"); // Redirige vers la page d'accueil après la suppression
        }      }      }

//        [HttpGet("user/{userId}")]
//        public async Task<IActionResult> GetMessagesByUserId(int userId)
//        {
//            var messages = await _messageService.GetMessagesByUserIdAsync(userId);
//            return Ok(messages);
//        }


//        [HttpGet("withAvatar")]
//        public async Task<IActionResult> GetMessagesWithAvatar()
//        {
//            var messages = await _messageService.GetMessagesWithAvatarAsync();
//            return Ok(messages);
//        }

//        [HttpPost("handleUserLoggedIn")]
//        public async Task<IActionResult> HandleUserLoggedIn([FromBody] int userId)
//        {
//            await _messageService.HandleUserLoggedInAsync(userId);
//            return Ok();
//        }

//    }
//}




