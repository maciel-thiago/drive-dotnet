using Drive.Domain.Auth;

namespace Drive.Business.Auth;

public class LoginHandler(IAuthService authService)
{
    public async Task<LoginResponse> HandleAsync(LoginRequest request, CancellationToken ct)
    {
        var token = await authService.LoginAsync(request.Email, request.Password, ct);
        return new LoginResponse { Token = token, ExpiresAt = DateTime.UtcNow.AddHours(1) };
    }
}
