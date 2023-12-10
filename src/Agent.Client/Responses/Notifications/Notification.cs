using System.Text.Json.Serialization;

namespace Agent.Client.Responses.Notifications {
    public record Notification(
        [property: JsonPropertyName("id")]
        string Id,
        [property: JsonPropertyName("created_at")]
        DateTimeOffset CreatedAt,
        [property: JsonPropertyName("message")]
        string Message,
        [property: JsonPropertyName("seen")]
        bool Seen,
        [property: JsonPropertyName("senderUserId")]
        string SenderUserId,
        [property: JsonPropertyName("receiverUserId")]
        string ReceiverUserId,
        [property: JsonPropertyName("type")]
        NotificationType Type
    );
}
