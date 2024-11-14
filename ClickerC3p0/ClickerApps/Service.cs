using ClickerC3p0.Database;
using Dapper;

namespace ClickerC3p0.ClickerApps;

public class ClickerAppService(IDBConnectionFactory dbConnectionFactory)
{
    public async Task<int> CreateApp(ClickerAppsCreateRequest newApp)
    {
        var dbConnection = await dbConnectionFactory.CreateConnectionAsync();
        var newId = await dbConnection.ExecuteAsync(
            """
            insert into clicker_apps (user_id, url, app_name, api_key_id)
            values (@UserId,@Url, @AppName, @ApiKeyId)
            """,
            newApp
        );
        return newId;
    }

    public async Task<IEnumerable<dynamic>> GetApps()
    {
        var dbConnection = await dbConnectionFactory.CreateConnectionAsync();
        var apps = await dbConnection.QueryAsync(
            """
            select * from clicker_apps
            """
        );
        return apps;
    }

    public async Task<IEnumerable<dynamic>> GetApp(int id)
    {
        var dbConnection = await dbConnectionFactory.CreateConnectionAsync();
        var app = await dbConnection.QueryAsync(
            """
            select * from clicker_apps where app_id=@Id
            """,
            new {id=id}
        );
        return app; 
    }

    public async Task<int> DeleteApp(int id)
    {
        var dbConnection = await dbConnectionFactory.CreateConnectionAsync();
        var appId = await dbConnection.ExecuteAsync(
            """
            delete from clicker_apps where app_id=@Id
            """,
            new {id=id}
        );
        return appId;
    }

    public async Task<int> UpdateApp(int id, ClickerAppsUpdateRequest app)
    {
        var dbConnection = await dbConnectionFactory.CreateConnectionAsync();
        var appId = await dbConnection.ExecuteAsync(
            """
            update clicker_apps 
            set  app_name = @AppName, url = @Url
            where app_id=@Id
            """,
            new {id=id, AppName=app.AppName, Url=app.Url}
        );
        return appId;
    }
}