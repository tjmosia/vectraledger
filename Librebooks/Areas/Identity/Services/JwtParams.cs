namespace VectraBooks.Areas.Identity.Services
{
    public class JwtParams
    {
        public string? SecretKey { get; set; }
        public string? Issuer { get; set; }
        public string? Audience { get; set; }
        public int ExpiryTimeInMinutes { get; set; }

        public JwtParams () { }

        public JwtParams (string? secretKey, string? issuer, string? audience, int expiryTimeInMinutes)
            : this()
        {
            SecretKey = secretKey;
            Issuer = issuer;
            Audience = audience;
            ExpiryTimeInMinutes = expiryTimeInMinutes;
        }

        public static (string SecretKey, string Issuer, string Audience, string ExpiryTimeInMinutes) GetKeyNames ()
            => (
                SecretKey: string.Concat(nameof(JwtParams), ":", nameof(SecretKey)),
                Issuer: string.Concat(nameof(JwtParams), ":", nameof(Issuer)),
                Audience: string.Concat(nameof(JwtParams), ":", nameof(Audience)),
                ExpiryTimeInMinutes: string.Concat(nameof(JwtParams), ":", nameof(ExpiryTimeInMinutes))
            );
    }
}
