using CardBooster.Blazor.Services.Interface;
using Microsoft.AspNetCore.Components.Authorization;

namespace CardBooster.Blazor.Services
{
    
    public class ApiAuthenticationStateProvider : AuthenticationStateProvider
    {
        private readonly JwtSecurityTokenHandler _tokenHandler = new();
        private ClaimsPrincipal _anonymous = new(new ClaimsIdentity());
        private string? _cachedToken;

        public bool IsAuthenticated => _cachedToken != null;

        public override Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            if (string.IsNullOrEmpty(_cachedToken))
            {
                return Task.FromResult(new AuthenticationState(_anonymous));
            }

            var claims = ParseClaimsFromJwt(_cachedToken);
            var identity = new ClaimsIdentity(claims, "jwt");
            var user = new ClaimsPrincipal(identity);

            return Task.FromResult(new AuthenticationState(user));
        }

        public Task MarkUserAsAuthenticated(string token)
        {
            _cachedToken = token;
            var claims = ParseClaimsFromJwt(token);
            var identity = new ClaimsIdentity(claims, "jwt");
            var user = new ClaimsPrincipal(identity);

            NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(user)));

            return Task.CompletedTask;
        }

        public Task MarkUserAsLoggedOut()
        {
            _cachedToken = null;
            NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(_anonymous)));

            return Task.CompletedTask;
        }

        private IEnumerable<Claim> ParseClaimsFromJwt(string jwt)
        {
            if (string.IsNullOrEmpty(jwt))
            {
                return Array.Empty<Claim>();
            }

            try
            {
                var token = _tokenHandler.ReadJwtToken(jwt);
                return token.Claims;
            }
            catch
            {
                return Array.Empty<Claim>();
            }
        }

    }
}
