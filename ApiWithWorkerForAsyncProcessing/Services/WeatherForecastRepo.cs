namespace ApiWithWorkerForAsyncProcessing.Services;

public class WeatherForecastRepo : IWeatherForecastRepo
{
    private static readonly List<WeatherForecast> _weatherForecasts = new();

    public Task<IEnumerable<WeatherForecast>> GetAllAsync()
    {
        return Task.FromResult(_weatherForecasts.AsEnumerable());
    }

    public Task InsertAsync(WeatherForecast weatherForecast)
    {
        _weatherForecasts.Add(weatherForecast);
        return Task.CompletedTask;
    }
}