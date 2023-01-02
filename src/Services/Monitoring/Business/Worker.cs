using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

public class Worker : IHostedService, IDisposable
{
    private int executionCount = 0;
    private readonly ILogger<Worker> _logger;
    private Timer? _timer;

    public Worker(ILogger<Worker> logger)
    {
        _logger = logger;
    }

    public void Dispose()
    {
        _timer?.Dispose();
    }

    public Task StartAsync(CancellationToken cancellationToken)
    {
        _logger.LogInformation("in StartAsync method");
        
        _timer = new Timer(Dowork, null, TimeSpan.Zero, TimeSpan.FromSeconds(1));

        return Task.CompletedTask;
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        _logger.LogInformation("Timed Hosted Service is stopping. Count: {executionCount}", executionCount);
        
        _timer?.Change(Timeout.Infinite, 0);

        return Task.CompletedTask;
    }

    private void Dowork(object? state)
    {
        var count = Interlocked.Increment(ref executionCount);

        _logger.LogInformation("Timed Hosted Service is working. Count: {Count}", count);
    }
}