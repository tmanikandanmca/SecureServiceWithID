using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text;
using System.Text.Encodings.Web;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.Extensions.Options;

namespace DigestAuth.Middleware;

public class DigestAuthenticationHandler(
    IOptionsMonitor<AuthenticationSchemeOptions> options,
    ILoggerFactory logger,
    UrlEncoder encoder)
    : AuthenticationHandler<AuthenticationSchemeOptions>(options, logger, encoder)
{

    protected override Task<AuthenticateResult> HandleAuthenticateAsync()
    {
 
        
        var authHeader = Request.Headers.Authorization.ToString();
        if (string.IsNullOrEmpty(authHeader) || !authHeader.StartsWith("Digest"))
            return Task.FromResult(AuthenticateResult.Fail("Missing / invalid data"));
        var claim = new[] { new Claim(ClaimTypes.Name, "digestUser") };
        var identity = new ClaimsIdentity(claim, Scheme.Name);
        var principal = new ClaimsPrincipal(identity);
        var ticket = new AuthenticationTicket(principal, Scheme.Name);
        return Task.FromResult(AuthenticateResult.Success(ticket));
    }
}