using System.Text.Json;
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

        _timer = new Timer(Dowork, null, TimeSpan.Zero, TimeSpan.FromSeconds(5));

        return Task.CompletedTask;
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        _logger.LogInformation("Timed Hosted Service is stopping. Count: {executionCount}", executionCount);

        _timer?.Change(Timeout.Infinite, 0);

        return Task.CompletedTask;
    }

    private async void Dowork(object? state)
    {
        using var client = new HttpClient();
        var result = await client.GetAsync("https://yesno.wtf/api");
        if (result.IsSuccessStatusCode)
        {
            var responseStr = await result.Content.ReadAsStringAsync();
            _logger.LogInformation(responseStr);
            var response = JsonSerializer.Deserialize<Root>(responseStr, new JsonSerializerOptions()
            {
                PropertyNameCaseInsensitive = true
            });
            _logger.LogInformation($"Answer is {response?.Answer}");
        }

        var count = Interlocked.Increment(ref executionCount);

        _logger.LogInformation("Timed Hosted Service is working. Count: {Count}", count);
    }
}

public class Root
{
    public string Answer { get; set; } = null!;
    public bool Forced { get; set; }
    public string Image { get; set; } = null!;
}