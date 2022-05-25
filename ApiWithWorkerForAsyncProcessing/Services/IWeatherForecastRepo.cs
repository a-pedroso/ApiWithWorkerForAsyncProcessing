namespace ApiWithWorkerForAsyncProcessing.Services;

public interface IWeatherForecastRepo
{
    Task InsertAsync(WeatherForecast weatherForecast);
    Task<IEnumerable<WeatherForecast>> GetAllAsync();
}
