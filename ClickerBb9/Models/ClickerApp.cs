namespace ClickerBb9.Models;

public record ClickerApp(
    int AppId,
    int UserId,
    string Url,
    string AppName,
    int AppKeyId,
    DateTime CreatedAt,
    DateTime UpdatedAt
    );