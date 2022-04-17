using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using BlazorSample.Shared;
using Microsoft.AspNetCore.Identity;
using BlazorSample.Server.Models;

namespace BlazorSample.Server.Controllers;

// The [Authorize] attribute indicates that the user must be authorized based on 
// the default policy to access the resource. The default authorization policy is 
// configured to use the default authentication scheme, which is set up by 
// AddIdentityServerJwt. 
[Authorize]
[ApiController]
[Route("[controller]")]
public class WeatherForecastController : ControllerBase
{
    private readonly UserManager<ApplicationUser> _userManager;

    private static readonly string[] Summaries = new[]
    {
        "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
    };

    private readonly ILogger<WeatherForecastController> _logger;

    public WeatherForecastController(ILogger<WeatherForecastController> logger, 
        UserManager<ApplicationUser> userManager)
    {
        _logger = logger;
        _userManager = userManager;
    }

    [HttpGet]
    public async Task<IEnumerable<WeatherForecast>> Get()
    {
        var rng = new Random();

        var user = await _userManager.GetUserAsync(User);

        if (user != null)
        {
            _logger.LogInformation($"User.Identity.Name: {user.UserName}");
        }

        return Enumerable.Range(1, 5).Select(index => new WeatherForecast
        {
            Date = DateTime.Now.AddDays(index),
            TemperatureC = rng.Next(-20, 55),
            Summary = Summaries[rng.Next(Summaries.Length)]
        })
        .ToArray();
    }
}
