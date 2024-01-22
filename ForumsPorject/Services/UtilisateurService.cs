using ForumsPorject.Repository.Entites;
using ForumsPorject.Repository.ClassesRepository;
using BCrypt.Net;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.SignalR;
using System.Data;

namespace ForumsPorject.Services
{
    public class UtilisateurService
    {
        private readonly UtilisateurRepository _utilisateurRepository;
        private readonly AppRoleService _appRoleService;
        private readonly JwtService _jwtService;
        private readonly AppRoleRepository _appRoleRepository;
        private readonly ILogger<UtilisateurService> _logger;
        private readonly IHttpContextAccessor _httpContextAccessor;


        public UtilisateurService(
            UtilisateurRepository utilisateurRepository,
            AppRoleService appRoleService,
            JwtService jwtService, AppRoleRepository appRoleRepository, ILogger<UtilisateurService> logger,
            IHttpContextAccessor httpContextAccessor)
        {
            _utilisateurRepository = utilisateurRepository;
            _appRoleService = appRoleService;
            _jwtService = jwtService;
            _appRoleRepository = appRoleRepository;
            _logger = logger;
            _httpContextAccessor = httpContextAccessor;
        }
        public async Task<bool> IsEmailUniqueAsync(string email)
        {
            return await _utilisateurRepository.IsEmailUniqueAsync(email);
        }
        public async Task<Utilisateur> CreateUtilisateurAsync(string pseudonyme, string email, string cheminavatar, string signature, string password)
        {
            // Hash password
            string hashedPassword = HashPassword(password);

            // Create user
            var utilisateur = new Utilisateur
            {
                Pseudonyme = pseudonyme,
                Email = email,
                Cheminavatar = cheminavatar,
                Signature = signature,
                Password = hashedPassword
            };

            await _utilisateurRepository.AddAsync(utilisateur);

            var defaultRole = await _appRoleRepository.CreateDefaultRoleAsync();

            // Création de l'association entre l'utilisateur et le rôle
            await _appRoleRepository.AddAppRoleToUtilisateurAsync(defaultRole, utilisateur);

            // Enregistrement des modifications dans la base de données
            await _utilisateurRepository.SaveChangesAsync();

            // Génération du jeton JWT
            var roles = await GetRolesForUserAsync(utilisateur.UtilisateurId);

            var token = _jwtService.GenerateToken(utilisateur);


            // En option, vous pouvez stocker le jeton dans l'entité utilisateur ou le renvoyer dans votre réponse.

            return utilisateur;
        }

        public async Task<Utilisateur> GetUserByIdAsync(int utilisateurId)
        {
            // Utilisez votre méthode GetByIdAsync du repository pour récupérer l'utilisateur par ID
            return await _utilisateurRepository.GetByIdAsync(utilisateurId);
        }
        public async Task<Utilisateur> Authentifier(string email, string motDePasse)
        {
            // Récupère l'utilisateur de la base de données.
            var utilisateur = await _utilisateurRepository.GetUtilisateurByEmail(email);

            if (utilisateur != null)
            {
                // Hash le mot de passe fourni par l'utilisateur avec le sel de la base de données.
                var motDePasseHachee = BCrypt.Net.BCrypt.HashPassword(motDePasse, utilisateur.Password);

                // Compare le mot de passe haché avec celui de la base de données.
                if (motDePasseHachee == utilisateur.Password)
                {
                    // Génère un jeton.
                    var roles = await GetRolesForUserAsync(utilisateur.UtilisateurId);
                   
                    var token = _jwtService.GenerateToken(utilisateur);

                    Console.WriteLine(token);
                    // Renvoie l'utilisateur.
                    return utilisateur;
                }
            }
            // Renvoie null si l'utilisateur n'est pas authentifié.
            return null;
        }
       
        public async Task<IList<string>> GetRolesForUserAsync(int utilisateurId)
        {
            return (IList<string>)await _utilisateurRepository.GetRolesForUserAsync(utilisateurId);
        }

        public async Task DeleteUtilisateurAsync(int id)
        {
            // Get user
            var utilisateur = await _utilisateurRepository.GetByIdAsync(id);

            if (utilisateur == null)
            {
                throw new InvalidOperationException("Utilisateur introuvable");
            }

            // Delete user
            _utilisateurRepository.Remove(utilisateur);
            await _utilisateurRepository.SaveChangesAsync();
        }

        private string HashPassword(string password)
        {
            return BCrypt.Net.BCrypt.HashPassword(password);
        }
        private bool VerifyPassword(string password, string hashedPassword)
        {
            return BCrypt.Net.BCrypt.Verify(password, hashedPassword);
        }

        //utilisée par la méthode Index
        public async Task<IEnumerable<Utilisateur>> GetAllWithRolesAsync()
        {
            // Utilisez GetAllAsync pour obtenir tous les utilisateurs
            var utilisateurs = await _utilisateurRepository.GetAllAsync();
            return utilisateurs;
        }

        public async Task<bool> UpdateUserAsync(int Id, string nouveauPseudonyme, string nouveauEamil, string nouveauAvatar, string nouvelleSignature, string nouveauPassword)
        {
            // Récupérer le message à mettre à jour
            var user = await _utilisateurRepository.GetByIdAsync(Id);
            string hashedPassword = HashPassword(nouveauPassword);
            // Vérifier si le message existe et si l'utilisateur est l'auteur du message
            if (user != null)
            {
                // Mettre à jour le contenu du message
                user.Pseudonyme= nouveauPseudonyme;
                user.Email= nouveauEamil;
                user.Cheminavatar= nouveauAvatar;
                user.Signature= nouvelleSignature;
                user.Password= hashedPassword;

               
                // Appeler la méthode de mise à jour du repository
                _utilisateurRepository.UpdateAsync(user);

                return true; // Mise à jour réussie
            }

            return false; // Message non trouvé ou utilisateur non autorisé
        }
       
        

    }
}


