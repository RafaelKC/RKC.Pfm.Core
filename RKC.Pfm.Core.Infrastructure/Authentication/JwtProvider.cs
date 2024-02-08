using System.Net.Http.Json;
using RKC.Pfm.Core.Infrastructure.Authentication.Dots;

namespace RKC.Pfm.Core.Infrastructure.Authentication;

public interface IJwtProvider
{
    Task<AuthToken> GetForCredentialsAsync(string email, string password);
}

public class JwtProvider: IJwtProvider
{
    private readonly HttpClient _httpClient;

    public JwtProvider(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<AuthToken> GetForCredentialsAsync(string email, string password)
    {
        var request = new
        {
            email,
            password,
            returnSecureToken = true
        };
        
        var response = await _httpClient.PostAsJsonAsync("", request);
        if (response.IsSuccessStatusCode)
        {
            return await response.Content.ReadFromJsonAsync<AuthToken>();
        }

        return new AuthToken();
    }
    
    
}