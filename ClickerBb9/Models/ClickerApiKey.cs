using System.Text.Json.Serialization;

namespace ClickerBb9.Models;

public record ClickerApiKey(
    int ApiKeyId,
    string Name,
    string HashValue,
    DateTime CreatedAt,
    DateTime UpdatedAt
);

public record ApisKeyResponse(List<ClickerApp> keys);


public record CreateClickerApisKeyRequest(
    [property: JsonPropertyName("name")]
    string Name
);

public record UpdateClickerApisKeyRequest(
    [property: JsonPropertyName("name")]
    string Name
);