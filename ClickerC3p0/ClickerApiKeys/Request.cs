using System.Text.Json.Serialization;

namespace ClickerC3p0.ClickerApiKeys;

public record ClickerApiKeysCreateRequest(
    [property: JsonPropertyName("name")]
    string Name
);

public record ClickerApiKeysUpdateRequest(
    [property: JsonPropertyName("name")]
    string Name
);