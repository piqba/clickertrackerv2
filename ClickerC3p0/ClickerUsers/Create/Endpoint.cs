namespace ClickerC3p0.ClickerUsers.Create;

public static class Endpoint
{
    public static WebApplication MapClickerUsersCreateEndpoint(this WebApplication app)
    {
        app.MapPost("/api/users", HandleAsync);
        return app;
    }

    private static Task<ResponseClickerUser> HandleAsync(RequestClickerUserCreate request)
    {
        return Task.FromResult(
            new ResponseClickerUser(
                Name: "user1",
                UserId: 1,
                CreatedAt: DateTime.Now,
                UpdatedAt: DateTime.Now
            )
        );
    }
}