namespace AuthService.Helpers
{
    public class JwtConfig
    {
        public TimeSpan TokenLifeTime { get; set; }
        public string ValidIssuer { get; set; }
        public string ValidAudience { get; set; }
        public string SecretKey { get; set; }
        public int RefreshTokenValidityinDays { get; set; }
    }
}

