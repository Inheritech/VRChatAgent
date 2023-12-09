using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace VrcEventAgent.Services {
    internal class AgentService(
        ILogger<AgentService> logger
    ) : IHostedService {

        public async Task StartAsync(CancellationToken cancellationToken) {
            logger.LogInformation("Agent is starting...");
        }

        public async Task StopAsync(CancellationToken cancellationToken) {
            logger.LogInformation("Agent is stopping...");
        }
    }
}
