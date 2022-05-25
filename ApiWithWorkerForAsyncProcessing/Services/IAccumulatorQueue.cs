using System.Diagnostics.CodeAnalysis;

namespace ApiWithWorkerForAsyncProcessing.Services;

public interface IAccumulatorQueue
{
    ValueTask<WeatherForecast> PullAsync(CancellationToken cancellationToken);
    ValueTask PushAsync([NotNull] WeatherForecast weatherForecast);
}
