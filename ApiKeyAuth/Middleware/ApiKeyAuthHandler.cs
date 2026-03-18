using System.Security.Claims;
using System.Text.Encodings.Web;
using ApiKeyAuth.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Options;

namespace ApiKeyAuth.Middleware;

public class ApiKeyAuthHandler(
    IOptionsMonitor<AuthenticationSchemeOptions> options,
    ILoggerFactory logger,
    UrlEncoder encoder,
    IConfiguration configuration)
    : AuthenticationHandler<AuthenticationSchemeOptions>(options, logger, encoder)
{
    private readonly IConfiguration _configuration = configuration; 
    
    protected override Task<AuthenticateResult> HandleAuthenticateAsync()
    {
         if( !Request.Headers.TryGetValue("X-API-KEY", out var apiKey  ))
             return Task.FromResult(AuthenticateResult.Fail("Missing API Key"));
         if (_configuration["ApiKey"] != apiKey)
             return Task.FromResult(AuthenticateResult.Fail("Invalid Auth Key"));
         var claims = new[] { new Claim(ClaimTypes.Name, "APIUser") };
         var identity = new ClaimsIdentity(claims, Scheme.Name);
         var principal = new ClaimsPrincipal(identity);
         var ticket = new AuthenticationTicket(principal, Scheme.Name);

         return Task.FromResult(AuthenticateResult.Success(ticket));

    }
}