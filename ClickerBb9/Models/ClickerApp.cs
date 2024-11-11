using System.Text.Json.Serialization;

namespace ClickerBb9.Models;

public record ClickerApp(
    [property: JsonPropertyName("app_id")]
    int AppId,
    [property: JsonPropertyName("user_id")]
    int UserId,
    string Url,
    [property: JsonPropertyName("app_name")]
    string AppName,
    [property: JsonPropertyName("app_key_id")]
    int AppKeyId,
    [property: JsonPropertyName("created_at")]
    DateTime CreatedAt,
    [property: JsonPropertyName("updated_at")]
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