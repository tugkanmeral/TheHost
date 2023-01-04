using System.Text.Json;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

public class Worker : IHostedService, IDisposable
{
    private readonly ILogger<Worker> _logger;
    private Timer? _timer;
    private readonly HashSet<ServiceInfo> SERVICES = new HashSet<ServiceInfo>()
    {
        new ServiceInfo("Note", "http://note:8080/healthCheck"),
        new ServiceInfo("User", "http://user:8080/healthCheck"),
        new ServiceInfo("Password", "http://password:8080/healthCheck"),
        new ServiceInfo("Tool", "http://tool:8080/healthCheck")
    };

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
        _timer = new Timer(Dowork, null, TimeSpan.Zero, TimeSpan.FromMinutes(1));

        return Task.CompletedTask;
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        _timer?.Change(Timeout.Infinite, 0);

        return Task.CompletedTask;
    }

    private async void Dowork(object? state)
    {
        using var client = new HttpClient();
        foreach (var service in SERVICES)
        {
            try
            {
                var result = await client.GetAsync($"{service.HealthCheckUrl}");
                if (result.IsSuccessStatusCode)
                {
                    var responseStr = await result.Content.ReadAsStringAsync();
                    _logger.LogInformation(service.Name + " : " + responseStr);
                }
                else
                {
                    _logger.LogWarning(service.Name + " : " + result.StatusCode.ToString());
                }
            }
            catch (System.Exception ex)
            {
                _logger.LogError(service.Name + " : " + ex.ToString());
            }
        }
    }
}

public record ServiceInfo(string Name, string HealthCheckUrl);