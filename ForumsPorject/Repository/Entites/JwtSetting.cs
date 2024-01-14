namespace ForumsPorject.Repository.Entites
{

    public class JwtSettings
    {
        public string Key { get; set; }
        public string Issuer { get; set; }
        public string Audience { get; set; }
        public int ExpirationInMinutes { get; set; }
        public List<string> Roles { get; set; }
    }

}
