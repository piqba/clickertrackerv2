using System.Text.Json.Serialization;

namespace ClickerC3p0.ClickerApps;

public record ClickerAppsCreateRequest(
    [property: JsonPropertyName("user_id")]
    int UserId,
    [property: JsonPropertyName("app_name")]
    string AppName,
    [property: JsonPropertyName("url")]
    string Url,
    [property: JsonPropertyName("api_key_id")]
    int ApiKeyId
);