using ApiWithWorkerForAsyncProcessing.Services;
using Microsoft.AspNetCore.Mvc;

namespace ApiWithWorkerForAsyncProcessing.Controllers;

[ApiController]
[Route("[controller]")]
public class WeatherForecastController : ControllerBase
{
    private readonly ILogger<WeatherForecastController> _logger;
    private readonly IAccumulatorQueue _accumulatorQueue;
    private readonly IWeatherForecastRepo _repo;

    public WeatherForecastController(
        ILogger<WeatherForecastController> logger,
        IAccumulatorQueue accumulatorQueue, 
        IWeatherForecastRepo repo)
    {
        _logger = logger;
        _accumulatorQueue = accumulatorQueue;
        _repo = repo;
    }

    [HttpGet(Name = "GetWeatherForecast")]
    public async Task<IActionResult> Get()
    {
        var list = await _repo.GetAllAsync();
        return Ok(list);
    }

    [HttpPost(Name = "CreateWeatherForecast")]
    public async Task<IActionResult> Post([FromBody] WeatherForecast weatherForecast)
    {
        _logger.LogInformation("creating WeatherForecast");
        await _accumulatorQueue.PushAsync(weatherForecast);
        return Accepted();
    }
}
