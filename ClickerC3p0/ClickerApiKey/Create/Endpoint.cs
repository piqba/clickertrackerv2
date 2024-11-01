namespace ClickerC3p0.ClickerApiKey.Create;

public static class Endpoint
{
    public static WebApplication MapClickerApiKeyCreateEndpoint(this WebApplication app)
    {
        app.MapPost("/api/keys", HandleAsync);
        return app;
    }

    private static Task<ResponseClickerApiKey> HandleAsync(RequestClickerApiKey request)
    {
        return Task.FromResult(
            new ResponseClickerApiKey(
                Name: "user1",
                ApiKeyId: 1,
                CreatedAt: DateTime.Now,
                UpdatedAt: DateTime.Now
            )
        );
    }
}