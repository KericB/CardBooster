using CardBooster.Blazor.Services.Interface;
using Microsoft.AspNetCore.Components;
using System.Net;
using System.Reflection.Metadata.Ecma335;

namespace CardBooster.Blazor.Services
{
    public class CustomAuthorizationMessageHandler : DelegatingHandler
    {
        private readonly IAuthService _authService;
        private readonly NavigationManager _navigationManager;

        public CustomAuthorizationMessageHandler(IAuthService authService, NavigationManager navigationManager)
        {
            _authService = authService;
            _navigationManager = navigationManager;
        }

        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            if (_authService.IsAuthenticated()) 
            { 
            
            }
            return base.SendAsync(request, cancellationToken);
        }

       

    }
}
