using System.Text;
using System.Text.Json;
using ClickerBb9.Models;

namespace ClickerBb9.Services;

public sealed class UserService
{
    private readonly HttpClient _httpClient;

    public UserService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<UsersResponse?> GetClickerUsersAsync()
    {
        var content = await _httpClient.GetFromJsonAsync<UsersResponse>("/api/users");
        return content;
    }
    public async Task<HttpResponseMessage> CreateClickerUserAsync(ClickerUserRequest request)
    {
        var data = JsonSerializer.Serialize(request);
        var jsonContent = new StringContent(data, Encoding.UTF8, "application/json");
        var content = await _httpClient.PostAsync("/api/users", jsonContent);
        return content;
    }
}