namespace ClickerC3p0.ClickerUsers.List;

public record Response(
    int NumberOfUsers,
    IReadOnlyCollection<ResponseClickerUser> Data);

public record ResponseClickerUser
(
    int UserId,
    string Name,
    DateTime CreatedAt,
    DateTime UpdatedAt
);