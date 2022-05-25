using ApiWithWorkerForAsyncProcessing.Services;

namespace ApiWithWorkerForAsyncProcessing.Worker;

public class AccumulatorBackgroundService : BackgroundService
{
    private readonly ILogger<AccumulatorBackgroundService> _logger;
    private readonly IAccumulatorQueue _queue;
    private readonly IWeatherForecastRepo _repo;

    public AccumulatorBackgroundService(ILogger<AccumulatorBackgroundService> logger,
      IAccumulatorQueue queue,
      IWeatherForecastRepo repo)
    {
        _logger = logger;
        _queue = queue;
        _repo = repo;
    }

    protected async override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                var weatherForecast = await _queue.PullAsync(stoppingToken);

                _logger.LogInformation($"Processing WeatherForecast for {weatherForecast.Id}");

                await _repo.InsertAsync(weatherForecast);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Failure while processing queue {ex}");
            }
        }
    }

    public override async Task StopAsync(CancellationToken cancellationToken)
    {
        _logger.LogInformation("Stopping Background Service");
        await base.StopAsync(cancellationToken);
    }
}
