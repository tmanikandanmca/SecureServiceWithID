using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text;
using System.Text.Encodings.Web;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace BasicAuth.Controllers;

public class AuthController(
    IOptionsMonitor<AuthenticationSchemeOptions> options,
    ILoggerFactory logger,
    UrlEncoder encoder)
    : AuthenticationHandler<AuthenticationSchemeOptions>(options, logger, encoder)
{
    protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
    {
        if (!Request.Headers.TryGetValue("Authorization", out var value))
            return await Task.FromResult(AuthenticateResult.Fail("Missing Authorization Header"));
        
        var authHeader =AuthenticationHeaderValue.Parse(value!);
        var credentialBytes = Convert.FromBase64String(authHeader.Parameter!);
        var credentials = Encoding.UTF8.GetString(credentialBytes).Split(':');
        var username = credentials[0];
        var password = credentials[1];


        if (username != "admin" || password != "password123")
            return await Task.FromResult(AuthenticateResult.Fail("Invalid Username or Password"));
        
        var claims=new[] { new Claim(ClaimTypes.Name,username ) };
        var identity = new ClaimsIdentity(claims, Scheme.Name);
        var principal = new ClaimsPrincipal(identity);
        var ticket = new AuthenticationTicket(principal, Scheme.Name);
        return await Task.FromResult(AuthenticateResult.Success(ticket));

    }
}