namespace ClickerBb9.Models;

public record ClickerApiKey(
    int ApiKeyId,
    string Name,
    string HashValue,
    DateTime CreatedAt,
    DateTime UpdatedAt
    );