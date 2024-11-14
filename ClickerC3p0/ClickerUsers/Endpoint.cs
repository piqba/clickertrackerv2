using ClickerC3p0.Database;
using Dapper;

namespace ClickerC3p0.ClickerUsers;

public static class Endpoint
{
    public static WebApplication MapClickerUsersEndpoints(this WebApplication app)
    {
        app.MapPost("/api/users", async (
            ClickerUserService svc,
            RequestClickerUserCreate request) =>
        {
            var newId  =await svc.CreateUser(request);
            return Results.Json(new { id = newId }, statusCode: StatusCodes.Status201Created);
        });

        app.MapGet("/api/users", async (ClickerUserService svc) =>
        {
            var users = await svc.GetUsers();
            return Results.Json(new { users }, statusCode: StatusCodes.Status200OK);
        });

        return app;
    }
}