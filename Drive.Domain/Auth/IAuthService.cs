namespace Drive.Domain.Auth;

public interface IAuthService
{
    Task<string> LoginAsync(string email, string password, CancellationToken ct = default);
}
