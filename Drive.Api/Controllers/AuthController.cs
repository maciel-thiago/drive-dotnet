using Drive.Business.Auth;
using Microsoft.AspNetCore.Mvc;

namespace Drive.Api.Controllers;

[ApiController]
[Route("auth")]
public class AuthController(LoginHandler loginHandler) : ControllerBase
{
    [HttpPost("login")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult<LoginResponse>> LoginAsync([FromBody] LoginRequest request)
    {
        try
        {
            var response = await loginHandler.HandleAsync(request, HttpContext.RequestAborted);
            return Ok(response);
        }
        catch (UnauthorizedAccessException)
        {
            return Unauthorized(new { error = "E-mail ou senha inválidos." });
        }
    }
}
