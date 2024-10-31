namespace ClickerC3p0.ClickerApps.List;

public record Response(
    int NumberOfApps,
    IReadOnlyCollection<ResponseClickerApp> Data);

public record ResponseClickerApp
(
    int AppId,
    int UserId,
    string Url,
    string AppName,
    int ApiKeyId,
    DateTime CreatedAt,
    DateTime UpdatedAt
);