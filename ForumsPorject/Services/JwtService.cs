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
        new Claim(ClaimTypes.NameIdentifier, utilisateur.UtilisateurId.ToString()),
        new Claim(ClaimTypes.Email, utilisateur.Email),
    };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.Key));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _jwtSettings.Issuer,
                audience: _jwtSettings.Audience,
                expires: DateTime.Now.AddMinutes(_jwtSettings.ExpirationInMinutes),
                signingCredentials: creds,
                claims: claims
            );

            //return new JwtSecurityTokenHandler().WriteToken(token);
            // Ajoutez le jeton dans un cookie sécurisé
            var tokenString = new JwtSecurityTokenHandler().WriteToken(token);
            _httpContextAccessor.HttpContext.Response.Cookies.Append("AccessToken", tokenString, new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.Strict,
                Expires = DateTime.Now.AddMinutes(_jwtSettings.ExpirationInMinutes)
            });

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

