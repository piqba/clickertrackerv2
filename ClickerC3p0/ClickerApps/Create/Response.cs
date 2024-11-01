namespace ClickerC3p0.ClickerApps.Create;

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