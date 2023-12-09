using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Serilog;

// Determine the environment
var environmentName = Environment.GetEnvironmentVariable("NETCORE_ENVIRONMENT") ?? "Production";

// Build configuration
var configurationBuilder = new ConfigurationBuilder()
    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);

// Add user secrets in development
if (environmentName == "Development") {
configurationBuilder.AddUserSecrets<Program>();
}

var configuration = configurationBuilder.Build();

// Configure Serilog
Log.Logger = new LoggerConfiguration()
    .ReadFrom.Configuration(configuration)
    .CreateLogger();

try {
Log.Information("Starting application");

var host = Host.CreateDefaultBuilder()
    .UseSerilog()
    .ConfigureServices((context, services) =>
{
services.AddHostedService<MyService>();
// Add other services and configurations here
})
    .Build();

await host.RunAsync();
} catch (Exception ex) {
Log.Fatal(ex, "Application terminated unexpectedly");
} finally {
Log.CloseAndFlush();
}

class MyService : IHostedService {
    private readonly ILogger _logger;

    public MyService(ILogger<MyService> logger) {
        _logger = logger;
    }

    public Task StartAsync(CancellationToken cancellationToken) {
        _logger.LogInformation("Service is starting");
        // Your continuous operation logic here

        return Task.CompletedTask;
    }

    public Task StopAsync(CancellationToken cancellationToken) {
        _logger.LogInformation("Service is stopping");
        // Your cleanup logic here

        return Task.CompletedTask;
    }
}