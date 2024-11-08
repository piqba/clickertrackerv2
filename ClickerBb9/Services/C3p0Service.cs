using System.Text;
using System.Text.Json;
using ClickerBb9.Models;

namespace ClickerBb9.Services;

public sealed class C3P0Service
{
    private readonly HttpClient _httpClient;

    public C3P0Service(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<UsersResponse?> GetClickerUser()
    {
        var content = await _httpClient.GetFromJsonAsync<UsersResponse>("/api/users");
        return content;
    }
    public async Task<HttpResponseMessage> CreateClickerUser(ClickerUserRequest clickerUserRequest)
    {
        var data = JsonSerializer.Serialize(clickerUserRequest);
        var jsonContent = new StringContent(data, Encoding.UTF8, "application/json");
        var content = await _httpClient.PostAsync("/api/users", jsonContent);
        return content;
    }
}