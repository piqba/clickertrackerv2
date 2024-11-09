using System.Text;
using System.Text.Json;
using ClickerBb9.Models;

namespace ClickerBb9.Services;

public sealed class AppService
{
    private readonly HttpClient _httpClient;

    public AppService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<AppsResponse?> GetClickerAppsAsync()
    {
        var content = await _httpClient.GetFromJsonAsync<AppsResponse>("/api/apps");
        return content;
    }

    public async Task<HttpResponseMessage> CreateClickerAppAsync(CreateClickerAppRequest request)
    {
        var data = JsonSerializer.Serialize(request);
        var jsonContent = new StringContent(data, Encoding.UTF8, "application/json");
        var content = await _httpClient.PostAsync("/api/apps", jsonContent);
        return content;
    }

    public async Task<HttpResponseMessage> UpdateClickerAppAsync(int id, UpdateClickerAppRequest request)
    {
        var data = JsonSerializer.Serialize(request);
        var jsonContent = new StringContent(data, Encoding.UTF8, "application/json");
        var content = await _httpClient.PutAsync($"/api/apps/{id}", jsonContent);
        return content;
    }

    public async Task<HttpResponseMessage> DeleteClickerAppAsync(int id)
    {
        var content = await _httpClient.DeleteAsync($"/api/apps/{id}");
        return content;
    }
}