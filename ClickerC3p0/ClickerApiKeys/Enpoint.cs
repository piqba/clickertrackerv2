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
            async (IDBConnectionFactory dbConnectionFactory, ClickerApiKeysCreateRequest request) =>
            {
                var dbConnection = await dbConnectionFactory.CreateConnectionAsync();
                var name = request.Name;
                var hashKey = ComputeSha256Hash(name);
                var newId = await dbConnection.ExecuteAsync(
                    """
                    insert into clicker_api_key (name,hash_value)
                    values (@name,@hash_value)
                    """,
                    new { name = name, hash_value = hashKey }
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

        app.MapDelete("/api/keys/{id}", async (int id, IDBConnectionFactory dbConnectionFactory) =>
        {
            var dbConnection = await dbConnectionFactory.CreateConnectionAsync();
            var rId = await dbConnection.ExecuteAsync(
                """
                    delete from clicker_api_key where api_key_id = @id
                """, new { id = id });
            return Results.Json(new { rId }, statusCode: StatusCodes.Status200OK);
        });
        app.MapGet("/api/keys/{id}", async (int id, IDBConnectionFactory dbConnectionFactory) =>
        {
            var dbConnection = await dbConnectionFactory.CreateConnectionAsync();
            var key = await dbConnection.QueryAsync(
                """
                   select * from clicker_api_key where api_key_id = @id limit 1
                """, new { id = id });

            return Results.Json(new { key }, statusCode: StatusCodes.Status200OK);
        });

        app.MapPut("/api/keys/{id}",
            async (int id, IDBConnectionFactory dbConnectionFactory, ClickerApiKeysUpdateRequest request) =>
            {
                var dbConnection = await dbConnectionFactory.CreateConnectionAsync();
                var key = await dbConnection.ExecuteAsync(
                    """
                       update clicker_api_key set name = @name where api_key_id = @id
                    """, new { id = id, name = request.Name });

                return Results.Json(new { key }, statusCode: StatusCodes.Status200OK);
            });
        return app;
    }

    static string ComputeSha256Hash(string rawData)
    {
        // Create a SHA256
        using (SHA256 sha256Hash = SHA256.Create())
        {
            // ComputeHash - returns byte array
            byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(rawData));

            // Convert byte array to a string
            StringBuilder builder = new StringBuilder();
            for (int i = 0; i < bytes.Length; i++)
            {
                builder.Append(bytes[i].ToString("x2"));
            }

            return builder.ToString();
        }
    }
}