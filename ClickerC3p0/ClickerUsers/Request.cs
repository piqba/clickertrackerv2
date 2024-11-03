using System.Text.Json.Serialization;

namespace ClickerC3p0.ClickerUsers;

public record RequestClickerUserCreate(
    [property: JsonPropertyName("name")]
    string Name
    );