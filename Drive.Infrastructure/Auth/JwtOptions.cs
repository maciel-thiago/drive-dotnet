namespace Drive.Infrastructure.Auth;

public class JwtOptions
{
    public string Issuer { get; set; } = null!;
    public string Audience { get; set; } = null!;
    public string SigningKey { get; set; } = null!;
    public int ExpirationMinutes { get; set; } = 60;
}
