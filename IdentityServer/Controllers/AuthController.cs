using IdentityServer.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/[controller]")]
public class AuthController(ITokenService tokenService) : ControllerBase
{
    [HttpPost("login")]
    public IActionResult Login([FromBody] LoginRequest request)
    {
        if (request.Email != "user@example.com" || request.Password != "password123")
            return Unauthorized(new { message = "Invalid credentials" });
        var token = tokenService.GenerateToken(
            userId: "user-id-123",
            email: request.Email,
            roles: ["User", "Admin"]
        );

        return Ok(new { token, expiresIn = 3600 });

    }

    [HttpGet("me")]
    [Authorize]
    public IActionResult GetCurrentUser()
    {
        var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
        var email = User.FindFirst(System.Security.Claims.ClaimTypes.Email)?.Value;
        var roles = User.FindAll(System.Security.Claims.ClaimTypes.Role)
            .Select(c => c.Value);

        return Ok(new { userId, email, roles });
    }
}

public record LoginRequest(string Email, string Password);