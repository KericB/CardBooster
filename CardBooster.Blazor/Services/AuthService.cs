using CardBooster.Blazor.Services.Interface;
using Microsoft.AspNetCore.Components.Authorization;
using System.Net.Http.Json;
using System.Text.Json;

namespace CardBooster.Blazor.Services
{
    public class AuthService : IAuthService
    {
        private readonly HttpClient _httpClient;
        private readonly AuthenticationStateProvider _authenticationStateProvider;  
        private readonly JsonSerializerOptions _jsonOptions= new() {
            PropertyNameCaseInsensitive = true,
        };
        public bool IsAuthenticated() => ((ApiAuthenticationStateProvider)_authenticationStateProvider).IsAuthenticated;

        public AuthService(HttpClient httpClient, AuthenticationStateProvider authenticationStateProvider)
        {
            _httpClient = httpClient;
            _authenticationStateProvider = authenticationStateProvider;
        }
        public async Task<string> login(string email, string password)
        {
            var content = JsonContent.Create(new
            {
                email,
                password
            });

            var response = await _httpClient.PostAsync("api/auth/login", content);

            if (response.IsSuccessStatusCode)
            {
                var authResult = await response.Content.ReadFromJsonAsync<LoginResult>(_jsonOptions);
                var token = authResult?.Token;

                if (!string.IsNullOrEmpty(token))
                {
                    await ((ApiAuthenticationStateProvider)_authenticationStateProvider).MarkUserAsAuthenticated(token);
                    _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
                }

            }
            return string.Empty;
        }

        public async Task Logout()
        {
            await((ApiAuthenticationStateProvider)_authenticationStateProvider).MarkUserAsLoggedOut();
            _httpClient.DefaultRequestHeaders.Authorization = null;
        }

        public async Task<bool> register(string email, string password)
        {
            var content = JsonContent.Create(new
            {
                email,
                password
            });
            
            var response = await _httpClient.PostAsync("api/auth/register", content);
            return response.IsSuccessStatusCode;
        }
    }
}
