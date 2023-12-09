using Agent.API.Services;
using Agent.Client.Responses.Notifications;
using Agent.Data;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Agent.API.Commands.Users {

    public record AcceptPendingFriendsCommand() : IRequest;

    public class AcceptPendingFriendsCommandHandler(VRChatServiceAccessor vrcContext, AgentDataContext dataContext, ILogger<AcceptPendingFriendsCommand> logger) : IRequestHandler<AcceptPendingFriendsCommand> {
        
        public async Task Handle(AcceptPendingFriendsCommand request, CancellationToken cancellationToken) {
            await vrcContext.EnsureLoggedInAsync();

            logger.LogInformation("Getting all notifications.");
            var pendingNotifications = await vrcContext.VRChat.GetNotificationsAsync();
            var pendingFriendRequests = pendingNotifications.Where(n => n.Type == NotificationType.FriendRequest).ToArray();

            logger.LogInformation("Processing {FriendRequestCount} friend requests", pendingFriendRequests.Length);
            foreach (var pendingFriendRequest in pendingFriendRequests) {
                var userId = pendingFriendRequest.SenderUserId;

                logger.LogInformation("Accepting friend request from {UserId}", userId);
                await vrcContext.VRChat.AcceptFriendRequestAsync(pendingFriendRequest.Id);
                var existingUser = await dataContext.Friends.FirstOrDefaultAsync(f => f.UserId == userId);
                if (existingUser != null) {
                    // Might be a user that deleted us and added us back
                    logger.LogInformation("User accepted and already registered skipping.");
                    continue;
                }

                await dataContext.Friends.AddAsync(new Data.Models.Friend(userId));
                logger.LogInformation("Registered user as friend.");
            }

            await vrcContext.VRChat.ClearNotificationsAsync();
        }
    }
}
