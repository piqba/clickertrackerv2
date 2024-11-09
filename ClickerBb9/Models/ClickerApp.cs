using System.Text.Json.Serialization;

namespace ClickerBb9.Models;

public record ClickerApp(
    int AppId,
    int UserId,
    string Url,
    string AppName,
    int AppKeyId,
    DateTime CreatedAt,
    DateTime UpdatedAt
);

public record CreateClickerAppRequest(
    [property: JsonPropertyName("user_id")]
    int UserId,
    [property: JsonPropertyName("url")]
    string Url,
    [property: JsonPropertyName("app_name")]
    string AppName,
    [property: JsonPropertyName("app_key_id")]
    int AppKeyId
);
public record UpdateClickerAppRequest(
    [property: JsonPropertyName("user_id")]
    int UserId,
    [property: JsonPropertyName("url")]
    string Url,
    [property: JsonPropertyName("app_name")]
    string AppName,
    [property: JsonPropertyName("app_key_id")]
    int AppKeyId
);

public record AppsResponse(List<ClickerApp> apps);