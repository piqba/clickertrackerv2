using System.Text;
using System.Text.Json;
using ClickerBb9.Models;

namespace ClickerBb9.Services;

public class ApiKeyService
{
    private readonly HttpClient _httpClient;

    public ApiKeyService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<ApisKeyResponse?> GetClickerApisKeyAsync()
    {
        var content = await _httpClient.GetFromJsonAsync<ApisKeyResponse>("/api/keys");
        return content;
    }

    public async Task<HttpResponseMessage> CreateClickerApiKeyAsync(CreateClickerApisKeyRequest request)
    {
        var data = JsonSerializer.Serialize(request);
        var jsonContent = new StringContent(data, Encoding.UTF8, "application/json");
        var content = await _httpClient.PostAsync("/api/keys", jsonContent);
        return content;
    }

    public async Task<HttpResponseMessage> UpdateClickerApisKeyAsync(int id, UpdateClickerApisKeyRequest request)
    {
        var data = JsonSerializer.Serialize(request);
        var jsonContent = new StringContent(data, Encoding.UTF8, "application/json");
        var content = await _httpClient.PutAsync($"/api/keys/{id}", jsonContent);
        return content;
    }

    public async Task<HttpResponseMessage> DeleteClickerApisKeyAsync(int id)
    {
        var content = await _httpClient.DeleteAsync($"/api/keys/{id}");
        return content;
    }
}