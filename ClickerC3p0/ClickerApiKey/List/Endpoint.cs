namespace ClickerC3p0.ClickerApiKey.List;

public static class Endpoint
{
    public static WebApplication MapApiKeyListEndpoint(this WebApplication app)
    {
        app.MapGet("/api/keys", HandleAsync);
        return app;
    }

    private static Task<Response> HandleAsync()
    {
        return Task.FromResult(
            new Response(
                1,
                new List<ResponseClickerApiKey>
                {
                    new ResponseClickerApiKey(
                        Name: "user1",
                        ApiKeyId: 1,
                        CreatedAt: DateTime.Now,
                        UpdatedAt: DateTime.Now
                    )
                }
            )
        );
    }
}