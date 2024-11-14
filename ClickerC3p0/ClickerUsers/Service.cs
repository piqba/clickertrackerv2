using ClickerC3p0.Database;
using Dapper;

namespace ClickerC3p0.ClickerUsers;

public class ClickerUserService(IDBConnectionFactory dbConnectionFactory)
{
    public async Task<int> CreateUser(RequestClickerUserCreate newUser)
    {
        using var dbConnection = await dbConnectionFactory.CreateConnectionAsync();
        var newId = await dbConnection.ExecuteAsync(
            """
            insert into clicker_users ( name)
            values (@name)
            """,
            new { name = newUser.Name }
        );
        return newId;
    }

    public async Task<IEnumerable<dynamic>> GetUsers()
    {
        using var dbConnection = await dbConnectionFactory.CreateConnectionAsync();
        var users = await dbConnection.QueryAsync(
            """
            select * from clicker_users
            """
        );
        return users;
    }
}