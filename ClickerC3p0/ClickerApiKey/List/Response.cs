namespace ClickerC3p0.ClickerApiKey.List;

public record Response(
    int NumberOfApps,
    IReadOnlyCollection<ResponseClickerApiKey> Data);

public record ResponseClickerApiKey
(
    string Name,
    int ApiKeyId,
    DateTime CreatedAt,
    DateTime UpdatedAt
);