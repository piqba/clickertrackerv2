using ClickerC3p0.Database;
using Dapper;

namespace ClickerC3p0.ClickerEvents;

public class ClickEventsService(IDBConnectionFactory dbConnectionFactory)
{

    public async Task<IEnumerable<dynamic>> GetClickerEventsAsync()
    {
        var dbConnection = await dbConnectionFactory.CreateConnectionAsync();
        var events = await dbConnection.QueryAsync(
            """
            select * from clicker_events_simple
            """
        );

        return events;
    }
}