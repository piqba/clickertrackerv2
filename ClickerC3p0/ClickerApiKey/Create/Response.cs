namespace ClickerC3p0.ClickerApiKey.Create;

public record ResponseClickerApiKey
(
    string Name,
    int ApiKeyId,
    DateTime CreatedAt,
    DateTime UpdatedAt
);