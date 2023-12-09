using Agent.API.Options;
using MailKit.Net.Imap;
using Microsoft.Extensions.Options;

namespace Agent.API.Services {
    public class EmailClientProvider(IOptions<VrcEmailOtpOptions> options, ILogger<EmailClientProvider> logger) {

        public async Task<ImapClient> GetClientAsync() {
            var currentOptions = options.Value;
            var client = new ImapClient();
            await client.ConnectAsync(currentOptions.ServerAddress, currentOptions.ServerPort, true);
            await client.AuthenticateAsync(currentOptions.Username, currentOptions.Password);
            return client;
        }
    }
}
