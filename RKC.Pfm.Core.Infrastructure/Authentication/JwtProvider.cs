using System.Net.Http.Json;
using Microsoft.Extensions.Configuration;
using RKC.Pfm.Core.Infrastructure.Authentication.Dots;
using RKC.Pfm.Core.Infrastructure.Consts;

namespace RKC.Pfm.Core.Infrastructure.Authentication;

public interface IJwtProvider
{
    Task<AuthToken> GetForCredentialsAsync(string email, string password);
}

public class JwtProvider: IJwtProvider
{
    private readonly HttpClient _httpClient;
    private readonly IConfiguration _configuration;

    public JwtProvider(HttpClient httpClient, IConfiguration configuration)
    {
        _httpClient = httpClient;
        _configuration = configuration;
    }

    public async Task<AuthToken> GetForCredentialsAsync(string email, string password)
    {
        var request = new
        {
            email,
            password,
            returnSecureToken = true
        };
        
        var response = await _httpClient.PostAsJsonAsync(_configuration[AppConfig.AuthenticationTokenUriKey], request);
        if (response.IsSuccessStatusCode)
        {
            return await response.Content.ReadFromJsonAsync<AuthToken>();
        }

        return new AuthToken();
    }
    
    
}