using System.Diagnostics.CodeAnalysis;
using System.Threading.Channels;

namespace ApiWithWorkerForAsyncProcessing.Services;

public class AccumulatorQueue : IAccumulatorQueue
{
    private readonly Channel<WeatherForecast> _queue;
    private readonly ILogger<AccumulatorQueue> _logger;

    public AccumulatorQueue(ILogger<AccumulatorQueue> logger)
    {
        var opts = new BoundedChannelOptions(100) { FullMode = BoundedChannelFullMode.Wait };
        _queue = Channel.CreateBounded<WeatherForecast>(opts);
        _logger = logger;
    }

    public async ValueTask PushAsync([NotNull] WeatherForecast weatherForecast)
    {
        await _queue.Writer.WriteAsync(weatherForecast);
        _logger.LogInformation("Added WeatherForecast to Queue");
    }

    public async ValueTask<WeatherForecast> PullAsync(CancellationToken cancellationToken)
    {
        var result = await _queue.Reader.ReadAsync(cancellationToken);
        _logger.LogInformation("Removed WeatherForecast from Queue");
        return result;
    }
}
