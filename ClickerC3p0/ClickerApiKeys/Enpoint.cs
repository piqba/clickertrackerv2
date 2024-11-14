using System.Security.Cryptography;
using System.Text;
using ClickerC3p0.Database;
using Dapper;

namespace ClickerC3p0.ClickerApiKeys;

public static class Endpoint
{
    public static WebApplication MapClickerApiKeysEndpoints(this WebApplication app)
    {
        app.MapPost("/api/keys",
            async (ClickerApiKeyService svc, ClickerApiKeysCreateRequest request) =>
            {
                var newId = await svc.CreateApiKey(request);
                return Results.Json(new { id = newId }, statusCode: StatusCodes.Status201Created);
            });
        app.MapGet("/api/keys", async (ClickerApiKeyService svc) =>
        {
            var keys = await svc.GetApiKeys();
            return Results.Json(new { keys }, statusCode: StatusCodes.Status200OK);
        });

        app.MapDelete("/api/keys/{id}", async (int id, ClickerApiKeyService svc) =>
        {
            var rId = await svc.DeleteApiKey(id);
            return Results.Json(new { rId }, statusCode: StatusCodes.Status200OK);
        });
        app.MapGet("/api/keys/{id}", async (int id, ClickerApiKeyService svc) =>
        {
            var key = await svc.GetApiKey(id);
            return Results.Json(new { key }, statusCode: StatusCodes.Status200OK);
        });

        app.MapPut("/api/keys/{id}",
            async (int id, ClickerApiKeyService svc, ClickerApiKeysUpdateRequest request) =>
            {
                var key = await svc.UpdateApiKey(id, request);

                return Results.Json(new { key }, statusCode: StatusCodes.Status200OK);
            });
        return app;
    }
}