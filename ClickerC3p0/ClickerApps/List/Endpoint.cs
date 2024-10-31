namespace ClickerC3p0.ClickerApps.List;

public static class Endpoint
{
    public static WebApplication MapClickerAppListEndpoint(this WebApplication app)
    {
        app.MapGet("/api/apps", HandleAsync);
        return app;
    }

    private static Task<Response> HandleAsync()
    {
        return Task.FromResult(
            new Response(
                1,
                new List<ResponseClickerApp>
                {
                    new ResponseClickerApp(
                        AppId: 1,
                        UserId: 1,
                        Url: "http://localhost:5000",
                        AppName: "Clicker",
                        ApiKeyId: 1,
                        CreatedAt: DateTime.Now,
                        UpdatedAt: DateTime.Now
                    )
                }
            )
        );
    }
}