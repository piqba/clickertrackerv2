using ClickerC3p0.Database;
using Dapper;

namespace ClickerC3p0.ClickerApps;

public static class Endpoint
{
    public static WebApplication MapClickerAppsEndpoints(this WebApplication app)
    {
        app.MapPost("/api/apps", async (IDBConnectionFactory dbConnectionFactory, ClickerAppsCreateRequest request) =>
        {
            var dbConnection = await dbConnectionFactory.CreateConnectionAsync();
            var newId = await dbConnection.ExecuteAsync(
                """
                insert into clicker_apps (user_id, url, app_name, api_key_id)
                values (@UserId,@Url, @AppName, @ApiKeyId)
                """,
                request
            );
            return Results.Json(new { id = newId }, statusCode: StatusCodes.Status201Created);
        });
        app.MapGet("/api/apps", async (IDBConnectionFactory dbConnectionFactory) =>
        {
            var dbConnection = await dbConnectionFactory.CreateConnectionAsync();
            var apps = await dbConnection.QueryAsync(
                """
                select * from clicker_apps
                """
            );
            return Results.Json(new { apps }, statusCode: StatusCodes.Status200OK);
        });

        app.MapGet("/api/apps/{id}", async (int id,IDBConnectionFactory dbConnectionFactory) =>
        {
            var dbConnection = await dbConnectionFactory.CreateConnectionAsync();
            var appId = await dbConnection.QueryAsync(
                """
                select * from clicker_apps where app_id=@Id
                """,
                new {id=id}
            );
            return Results.Json(new { appId }, statusCode: StatusCodes.Status200OK);

        });
        app.MapDelete("/api/apps/{id}", async (int id,IDBConnectionFactory dbConnectionFactory) =>
        {
            var dbConnection = await dbConnectionFactory.CreateConnectionAsync();
            var appId = await dbConnection.ExecuteAsync(
                """
                delete from clicker_apps where app_id=@Id
                """,
                new {id=id}
            );
            return Results.Json(new { appId }, statusCode: StatusCodes.Status200OK);

        });
        app.MapPut("/api/apps/{id}", async (int id,IDBConnectionFactory dbConnectionFactory, ClickerAppsUpdateRequest request) =>
        {
            var dbConnection = await dbConnectionFactory.CreateConnectionAsync();
            var appId = await dbConnection.ExecuteAsync(
                """
                update clicker_apps 
                set  app_name = @AppName, url = @Url
                where app_id=@Id
                """,
                new {id=id, AppName=request.AppName, Url=request.Url}
            );
            return Results.Json(new { appId }, statusCode: StatusCodes.Status200OK);

        });
        return app;
    }
}