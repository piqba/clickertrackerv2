using ClickerC3p0.Database;
using Dapper;

namespace ClickerC3p0.ClickerEvents;

public static class Endpoint
{
    public static WebApplication MapClickerEventsEndpoint(this WebApplication app)
    {
        app.MapGet("/api/clicker-events", async (IDBConnectionFactory dbConnectionFactory) =>
        {
            var dbConnection = await dbConnectionFactory.CreateConnectionAsync();
            var events = await dbConnection.QueryAsync(
                """
                select * from clicker_events_simple
                """
            );
            return Results.Json(new { events }, statusCode: StatusCodes.Status200OK);
        });
        return app;
    }

  
}