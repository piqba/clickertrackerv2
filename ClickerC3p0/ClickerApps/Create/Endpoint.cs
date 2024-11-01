namespace ClickerC3p0.ClickerApps.Create;

public static class Endpoint
{
    public static WebApplication MapClickerAppCreateEndpoint(this WebApplication app)
    {
        app.MapPost("/api/apps", HandleAsync);
        return app;
    }

    private static Task<ResponseClickerApp> HandleAsync(RequestClickerApps request)
    {
        return Task.FromResult(
            new ResponseClickerApp(
                AppId: 1,
                UserId: 1,
                Url: "http://localhost:5000",
                AppName: "Clicker",
                ApiKeyId: 1,
                CreatedAt: DateTime.Now,
                UpdatedAt: DateTime.Now
            )
        );
    }
}