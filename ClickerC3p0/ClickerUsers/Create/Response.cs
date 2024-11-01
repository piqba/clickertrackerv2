namespace ClickerC3p0.ClickerUsers.Create;

public record ResponseClickerUser
(
    int UserId,
    string Name,
    DateTime CreatedAt,
    DateTime UpdatedAt
);