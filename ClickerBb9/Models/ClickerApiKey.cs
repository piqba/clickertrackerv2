using System.Text.Json.Serialization;

namespace ClickerBb9.Models;

public record ClickerApiKey(
    [property: JsonPropertyName("api_key_id")]
    int ApiKeyId,
    string Name,
    [property: JsonPropertyName("hash_value")]
    string HashValue,
    [property: JsonPropertyName("created_at")]
    DateTime CreatedAt,
    [property: JsonPropertyName("updated_at")]
    DateTime UpdatedAt
);

public record ApisKeyResponse(List<ClickerApiKey> keys);


public record CreateClickerApisKeyRequest(
    [property: JsonPropertyName("name")]
    string Name
);

public record UpdateClickerApisKeyRequest(
    [property: JsonPropertyName("name")]
    string Name
);