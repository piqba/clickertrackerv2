using System.Text.Json.Serialization;

namespace ClickerBb9.Models;

public record ClickerUser(
    [property: JsonPropertyName("user_id")]
    int UserId,
    string Name,
    [property: JsonPropertyName("created_at")]
    DateTime CreatedAt,
    [property: JsonPropertyName("updated_at")]
    DateTime UpdatedAt
);

public record UsersResponse(List<ClickerUser> users);

public record ClickerUserRequest(string Name);

