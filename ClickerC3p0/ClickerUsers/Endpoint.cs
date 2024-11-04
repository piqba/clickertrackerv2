using ClickerC3p0.Database;
using Dapper;

namespace ClickerC3p0.ClickerUsers;

public static class Endpoint
{
    public static WebApplication MapClickerUsersEndpoints(this WebApplication app)
    {
        app.MapPost("/api/users", async (IDBConnectionFactory dbConnectionFactory,
            RequestClickerUserCreate request) =>
        {
            using var dbConnection = await dbConnectionFactory.CreateConnectionAsync();
            var newId = await dbConnection.ExecuteAsync(
                """
                insert into clicker_users ( name)
                values (@name)
                """,
                new { name = request.Name }
            );
            return Results.Json(new { id = newId }, statusCode: StatusCodes.Status201Created);
        });

        app.MapGet("/api/users", async (IDBConnectionFactory dbConnectionFactory) =>
        {
            using var dbConnection = await dbConnectionFactory.CreateConnectionAsync();
            var users = await dbConnection.QueryAsync(
                """
                select * from clicker_users
                """
            );
            return Results.Json(new { users }, statusCode: StatusCodes.Status200OK);
        });

        return app;
    }
}