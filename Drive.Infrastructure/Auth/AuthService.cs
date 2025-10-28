using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Drive.Domain.Auth;
using Drive.Domain.Interfaces;

namespace Drive.Infrastructure.Auth;

public class AuthService(IUserRepository repository, JwtOptions jwtOptions) : IAuthService
{
    public Task<string> LoginAsync(string email, string password, CancellationToken ct = default)
    {
        var user = repository.GetByEmailAsync(email, ct);

        if (user is null)
            throw new UnauthorizedAccessException("Credenciais inválidas");

        /*var isValidPassword = BCrypt.Net.BCrypt.Verify(password, user.);
        if (!isValidPassword)
            throw new UnauthorizedAccessException("Credenciais inválidas");*/

        /*new List<Claim>(
            new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
            new Claim(JwtRegisteredClaimNames.Email, user),
            new Claim("uid")
            );*/

        return Task.FromResult("token");
    }
}
