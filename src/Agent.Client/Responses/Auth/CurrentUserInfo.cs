using System.Text.Json.Serialization;

namespace Agent.Client.Responses.Auth
{
    public record CurrentUserInfo(
        [property: JsonPropertyName("id")]
        string Id,
        [property: JsonPropertyName("displayName")]
        string DisplayName,
        [property: JsonPropertyName("username")]
        string Username
    );
}
