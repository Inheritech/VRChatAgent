
using Agent.API.Commands.Users;
using Agent.API.Services;
using MediatR;

namespace Agent.API.HostedServices {
    public class NotificationProcessingService(IServiceProvider services, ILogger<NotificationProcessingService> logger) : BackgroundService {
        protected override async Task ExecuteAsync(CancellationToken stoppingToken) {
            logger.LogInformation("Starting notification processing.");
            while(!stoppingToken.IsCancellationRequested) {
                using (var scope = services.CreateScope()) {
                    var mediator = scope.ServiceProvider.GetRequiredService<IMediator>();
                    await mediator.Send(new AcceptPendingFriendsCommand(), stoppingToken);
                }
                if (stoppingToken.IsCancellationRequested) {
                    return;
                }
                await Task.Delay(60 * 1000, stoppingToken);
            }
        }
    }
}
