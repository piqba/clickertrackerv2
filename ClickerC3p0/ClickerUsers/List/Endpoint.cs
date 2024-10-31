namespace ClickerC3p0.ClickerUsers.List;

public static class Endpoint
{
    public static WebApplication MapUsersListEndpoint(this WebApplication app)
    {
        app.MapGet("/api/users", HandleAsync);
        return app;
    }

    private static Task<Response> HandleAsync()
    {
        return Task.FromResult(
            new Response(
                1,
                new List<ResponseClickerUser>
                {
                    new ResponseClickerUser(
                        Name: "user1",
                        UserId: 1,
                        CreatedAt: DateTime.Now,
                        UpdatedAt: DateTime.Now
                    )
                }
            )
        );
    }
}