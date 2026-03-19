using System.Security.Claims;
using System.Text.Encodings.Web;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Options;

namespace ApiKeyAuth.Middleware;

 
public class RbApiKeyAuthHandler(
    IOptionsMonitor<AuthenticationSchemeOptions> options,
    ILoggerFactory logger,
    UrlEncoder encoder,
    ISystemClock clock,
    IConfiguration configuration)
    : AuthenticationHandler<AuthenticationSchemeOptions>(options, logger, encoder, clock)
{
    private readonly IConfiguration _configuration = configuration;
    
    protected override Task<AuthenticateResult> HandleAuthenticateAsync()
    {
        var apiKey = Request.Headers["X-API-KEY"].FirstOrDefault();
        var keys = _configuration.GetSection("RoleApiKeys").Get<List<ApiKeyConfig>>();

        
        if( string.IsNullOrEmpty(apiKey))
            return Task.FromResult(AuthenticateResult.Fail("Missing API Key"));
        if (_configuration["ApiKey"] != apiKey)
            return Task.FromResult(AuthenticateResult.Fail("Invalid Auth Key"));
        
        var match = keys?.FirstOrDefault(k => k.Key == apiKey);

        if (match == null)
            return Task.FromResult(AuthenticateResult.Fail("Invalid API Key"));
       
        var claims = match.Roles.Select(r => new Claim(ClaimTypes.Role, r)).ToList();
        claims.Add(new Claim(ClaimTypes.Name, "ApiKeyUser"));

        var identity = new ClaimsIdentity(claims, Scheme.Name);
        var principal = new ClaimsPrincipal(identity);
        var ticket = new AuthenticationTicket(principal, Scheme.Name);

        return Task.FromResult(AuthenticateResult.Success(ticket));
       
    }
}

public class ApiKeyConfig
{
    public string Key { get; set; }
    public List<string> Roles { get; set; }
}