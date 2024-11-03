using ClickerC3p0.Database;
using Dapper;

namespace ClickerC3p0.ClickerApiKeys;

public static class Endpoint
{
    public static WebApplication MapClickerApiKeysEndpoints(this WebApplication app)
    {
        app.MapPost("/api/keys", async (IDBConnectionFactory dbConnectionFactory, ClickerApiKeysCreateRequest request) =>
        {
            var dbConnection = await dbConnectionFactory.CreateConnectionAsync();
            var newId = await dbConnection.ExecuteAsync(
                """
                insert into clicker_api_key (name)
                values (@Name)
                """,
                request
            );
            return Results.Json(new { id = newId }, statusCode: StatusCodes.Status201Created);
        });
        app.MapGet("/api/keys", async (IDBConnectionFactory dbConnectionFactory) =>
        {
            var dbConnection = await dbConnectionFactory.CreateConnectionAsync();
            var keys = await dbConnection.QueryAsync(
                """
                select * from clicker_api_key
                """
            );
            return Results.Json(new { keys }, statusCode: StatusCodes.Status200OK);
        });
        return app;
    }
}