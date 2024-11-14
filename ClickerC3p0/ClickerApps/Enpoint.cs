using ClickerC3p0.Database;
using Dapper;

namespace ClickerC3p0.ClickerApps;

public static class Endpoint
{
    public static WebApplication MapClickerAppsEndpoints(this WebApplication app)
    {
        app.MapPost("/api/apps", async (ClickerAppService svc, ClickerAppsCreateRequest request) =>
        {
            var newId = await svc.CreateApp(request);
            return Results.Json(new { id = newId }, statusCode: StatusCodes.Status201Created);
        });
        app.MapGet("/api/apps", async (ClickerAppService svc) =>
        {
            var apps = await svc.GetApps();
            return Results.Json(new { apps }, statusCode: StatusCodes.Status200OK);
        });

        app.MapGet("/api/apps/{id}", async (int id, ClickerAppService svc) =>
        {
            var foundApp = await svc.GetApp(id);
            return Results.Json(new { app = foundApp }, statusCode: StatusCodes.Status200OK);
        });
        app.MapDelete("/api/apps/{id}", async (int id, ClickerAppService svc) =>
        {
            var appId = await svc.DeleteApp(id);
            return Results.Json(new { appId }, statusCode: StatusCodes.Status200OK);
        });
        app.MapPut("/api/apps/{id}", async (int id, ClickerAppService svc, ClickerAppsUpdateRequest request) =>
        {
            var appId = await svc.UpdateApp(id, request);
            return Results.Json(new { appId }, statusCode: StatusCodes.Status200OK);
        });
        return app;
    }
}