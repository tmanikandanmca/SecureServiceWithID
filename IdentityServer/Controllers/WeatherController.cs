using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace IdentityServer.Controllers;

[ApiController]
[Route("api/[controller]")]
public class WeatherController : ControllerBase
{
    [HttpGet]
    [Authorize]
    public IActionResult GetWeather()
    {
        var forecast = new[] { "Sunny", "Cloudy", "Rainy" };
        return Ok(forecast);
    }
}