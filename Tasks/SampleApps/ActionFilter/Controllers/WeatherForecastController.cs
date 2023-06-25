using ActionFilters.Data;
using Microsoft.AspNetCore.Mvc;


namespace ActionFilter.Controllers;


[ApiController]
[Route("[controller]")]
public class UserController : ControllerBase
{
   public string Get()
    {
        return "User Page";
    }
    [Route("{id:int:range(10,20)}")]
    public string GetById(int id)
    {
        return "Hello Int : " + id;
    }

    [Route("{id:regex(a(b|c))}")]
    public string GetByIdString(string id)
    {
        return "Hello String : " + id;
    }
}
//regex(a(b|c))
//minlength(5)

[ApiController]
[Route("[controller]")]
//[CustomActionFilterAttribute("Controller")]

public class WeatherForecastController : ControllerBase
{
    private static readonly string[] Summaries = new[]
    {
        "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
    };

    private readonly ILogger<WeatherForecastController> _logger;

    public WeatherForecastController(ILogger<WeatherForecastController> logger)
    {
        _logger = logger;
    }

    [HttpGet(Name = "GetWeatherForecast")]
    public IEnumerable<WeatherForecast> Get()
    {
        return Enumerable.Range(1, 5).Select(index => new WeatherForecast
        {
            Date = DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
            TemperatureC = Random.Shared.Next(-20, 55),
            Summary = Summaries[Random.Shared.Next(Summaries.Length)]
        })
        .ToArray();
    }
    [TypeFilter(typeof(CustomActionFilter))]
     public class ExceptionController : Controller
    {
    public IActionResult Index() =>
        Content($"- {nameof(ExceptionController)}.{nameof(Index)}");
    }      
}





