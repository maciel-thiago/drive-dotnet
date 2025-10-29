using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Drive.Domain.Auth;
using Drive.Domain.Interfaces;
using Microsoft.IdentityModel.Tokens;

namespace Drive.Infrastructure.Auth;

public class AuthService(IUserRepository repository, JwtOptions jwtOptions) : IAuthService
{
    public async Task<string> LoginAsync(
        string email,
        string password,
        CancellationToken ct = default
    )
    {
        var user = await repository.GetByEmailAsync(email, ct);

        if (user is null)
            throw new UnauthorizedAccessException("Credenciais inválidas");

        var isValidPassword = BCrypt.Net.BCrypt.Verify(password, user.PasswordHash);
        if (!isValidPassword)
            throw new UnauthorizedAccessException("Credenciais inválidas");

        var claims = new List<Claim>
        {
            new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
            new Claim(JwtRegisteredClaimNames.Email, user.Email),
            new Claim("uid", user.Id.ToString()),
        };

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtOptions.SigningKey));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var expires = DateTime.UtcNow.AddMinutes(jwtOptions.ExpirationMinutes);

        var token = new JwtSecurityToken(
            issuer: jwtOptions.Issuer,
            audience: jwtOptions.Audience,
            claims: claims,
            notBefore: DateTime.UtcNow,
            expires: expires,
            signingCredentials: creds
        );

        var encoded = new JwtSecurityTokenHandler().WriteToken(token);
        return encoded;
    }
}
