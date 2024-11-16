using System.Security.Cryptography;
using System.Text;
using ClickerC3p0.Database;
using Dapper;

namespace ClickerC3p0.ClickerApiKeys;

public class ClickerApiKeyService(IDbConnectionFactory dbConnectionFactory)
{
    public async Task<int> CreateApiKey(ClickerApiKeysCreateRequest apikey)
    {
        var dbConnection = await dbConnectionFactory.CreateConnectionAsync();
        var name = apikey.Name;
        var hashKey = ComputeSha256Hash(name);
        var newId = await dbConnection.ExecuteAsync(
            """
            insert into clicker_api_key (name,hash_value)
            values (@name,@hash_value)
            """,
            new { name = name, hash_value = hashKey }
        );
        return newId;
    }

    public async Task<IEnumerable<dynamic>> GetApiKeys()
    {
        var dbConnection = await dbConnectionFactory.CreateConnectionAsync();
        var keys = await dbConnection.QueryAsync(
            """
            select * from clicker_api_key
            """
        );
        return keys;
    }

    public async Task<int> DeleteApiKey(int id)
    {
        var dbConnection = await dbConnectionFactory.CreateConnectionAsync();
        var rId = await dbConnection.ExecuteAsync(
            """
                delete from clicker_api_key where api_key_id = @id
            """, new { id = id });
        return rId;
    }

    public async Task<IEnumerable<dynamic>> GetApiKey(int id)
    {
        var dbConnection = await dbConnectionFactory.CreateConnectionAsync();
        var key = await dbConnection.QueryAsync(
            """
               select * from clicker_api_key where api_key_id = @id limit 1
            """, new { id = id });
        return key;
    }

    public async Task<int> UpdateApiKey(int id, ClickerApiKeysUpdateRequest apikey)
    {
        var dbConnection = await dbConnectionFactory.CreateConnectionAsync();
        var key = await dbConnection.ExecuteAsync(
            """
               update clicker_api_key set name = @name where api_key_id = @id
            """, new { id = id, name = apikey.Name });
        return key;
    }

    private string ComputeSha256Hash(string rawData)
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