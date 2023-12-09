using Agent.API.Exceptions;
using Agent.Client;
using MailKit;
using MailKit.Search;
using System.Text.RegularExpressions;

namespace Agent.API.Services {
    public partial class VRChatServiceAccessor {
        public const string VrChatNoReplyAddress = "noreply@vrchat.com";
        public VRChatService VRChat { get; }

        private readonly EmailClientProvider _emailClientProvider;

        private readonly ILogger<VRChatServiceAccessor> _logger;

        public VRChatServiceAccessor(EmailClientProvider emailProvider, VRChatService vrChat, ILogger<VRChatServiceAccessor> logger) {
            _emailClientProvider = emailProvider;
            VRChat = vrChat;

            _logger = logger;
        }

        private async Task<string> GetEmailOtpCodeAsync() {
            var cancellationTokenSource = new CancellationTokenSource(2 * 60 * 1000);
            using (var email = await _emailClientProvider.GetClientAsync()) {
                var inbox = email.Inbox;
                await inbox.OpenAsync(FolderAccess.ReadOnly);

                var searchQuery = SearchQuery.FromContains(VrChatNoReplyAddress);
                var orderBy = new OrderBy[] { OrderBy.ReverseArrival };

                while (!cancellationTokenSource.IsCancellationRequested) {
                    var messageUids = await inbox.SearchAsync(searchQuery);
                    var sortedUids = await inbox.SortAsync(messageUids, searchQuery, orderBy);
                    foreach (var summary in await inbox.FetchAsync(sortedUids.Take(10).ToList(), MessageSummaryItems.Envelope)) {
                        if (summary.Envelope.Date > DateTimeOffset.Now.AddMinutes(-2)) {
                            var subjectMatch = VRChatEmailOtpSubjectRegex().Match(summary.Envelope.Subject);
                            if (subjectMatch.Success) {
                                return subjectMatch.Groups[^1].Value;
                            }
                        }
                    }

                    await Task.Delay(5 * 1000);
                }
            }
            throw new AgentOperationException("Did not receive Email OTP in a timely manner, increase time in settings if needed.");
        }

        private async Task HandleEmailOtpAsync() {
            // Wait two minutes tops
            var emailOtp = await GetEmailOtpCodeAsync();
            var verifyResultResponse = await VRChat.VerifyTwoFactorEmailOtp(emailOtp);
            if (!verifyResultResponse.Verified) {
                throw new AgentOperationException("Failed to verify email.");
            }

            _logger.LogInformation("Email OTP verified.");
        }

        public async Task EnsureLoggedInAsync() {

            var loginResult = await VRChat.LoginAsync();
            if (loginResult == LoginResult.RequiresEmailOtp) {
                await HandleEmailOtpAsync();
                await VRChat.LoginAsync();
            }

            var currentUser = VRChat.CurrentUser ?? throw new AgentOperationException("Failed to login.");

            _logger.LogInformation("Logged in as {UserId}", currentUser.Id);
        }

        [GeneratedRegex(@"Your One-Time Code is (\d{6})")]
        private static partial Regex VRChatEmailOtpSubjectRegex();
    }
}
