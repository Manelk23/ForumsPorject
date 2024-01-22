using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using ForumsPorject.Repository.Entites;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace ForumsPorject.Services
{
    public class JwtService
    {
        private readonly JwtSettings _jwtSettings;
        private readonly IHttpContextAccessor _httpContextAccessor;


        public JwtService(IOptions<JwtSettings> jwtSettings, IHttpContextAccessor httpContextAccessor)
        {
            _jwtSettings = jwtSettings.Value;
            _httpContextAccessor = httpContextAccessor;
        }

        public string GenerateToken(Utilisateur utilisateur)
        {
            // Claims pour le payload du token
            var claims = new List<Claim>
        {
        // Ajoute l'ID de l'utilisateur au payload
        new Claim(ClaimTypes.NameIdentifier, utilisateur.UtilisateurId.ToString()),
        
        // Ajoute l'email de l'utilisateur au payload
        new Claim(ClaimTypes.Email, utilisateur.Email),
        };

            // Clé secrète utilisée pour signer le token
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.Key));

            // Crée un objet SigningCredentials avec la clé et l'algorithme de chiffrement HMACSHA256
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            // Crée un jeton JWT avec les informations spécifiées
            var token = new JwtSecurityToken(
                issuer: _jwtSettings.Issuer,                    // Émetteur du jeton
                audience: _jwtSettings.Audience,                // Audience du jeton
                expires: DateTime.Now.AddMinutes(_jwtSettings.ExpirationInMinutes), // Date d'expiration du jeton
                signingCredentials: creds,                       // Informations d'identification pour la signature du jeton
                claims: claims                                  // Informations supplémentaires incluses dans le jeton
            );

            // Crée une chaîne de caractères à partir du jeton JWT
            var tokenString = new JwtSecurityTokenHandler().WriteToken(token);

            // Ajoute le jeton dans un cookie sécurisé
            _httpContextAccessor.HttpContext.Response.Cookies.Append("AccessToken", tokenString, new CookieOptions
            {
                HttpOnly = true,                              // Le cookie est accessible uniquement via HTTP
                Secure = true,                                // Le cookie ne sera envoyé que sur des connexions HTTPS
                SameSite = SameSiteMode.Strict,               // Restreint l'envoi du cookie aux requêtes du même site
                Expires = DateTime.Now.AddMinutes(_jwtSettings.ExpirationInMinutes) // Date d'expiration du cookie
            });

            // Retourne la chaîne de caractères du jeton JWT
            return tokenString;
        }

        public ClaimsPrincipal DecodeToken(string token)
        {
            var handler = new JwtSecurityTokenHandler();
            var jsonToken = handler.ReadToken(token) as JwtSecurityToken;

            if (jsonToken == null)
                throw new ArgumentException("Invalid JWT token");

            var principal = new ClaimsPrincipal(new ClaimsIdentity(jsonToken.Claims, "jwt"));

            return principal;
        }


    }

    
}

