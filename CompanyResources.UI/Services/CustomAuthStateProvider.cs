using Microsoft.AspNetCore.Components.Authorization;
using System.Net.Http.Headers;
using System.Security.Claims;

namespace CompanyResources.UI.Services
{
    public class CustomAuthStateProvider : AuthenticationStateProvider
    {
        private readonly HttpClient _httpClient;
        // W prawdziwej appce użyj LocalStorage, tu trzymamy w pamięci RAM dla prostoty
        private string _authToken = string.Empty;

        public CustomAuthStateProvider(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public override async Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            var identity = new ClaimsIdentity();

            if (!string.IsNullOrEmpty(_authToken))
            {
                // Dekodowanie uproszczone (zakładamy że token jest ok)
                identity = new ClaimsIdentity(new[]
                {
                new Claim(ClaimTypes.Name, "User"),
            }, "jwt");
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _authToken);
            }
            else
            {
                _httpClient.DefaultRequestHeaders.Authorization = null;
            }

            var user = new ClaimsPrincipal(identity);
            return new AuthenticationState(user);
        }

        public void NotifyUserLoggedIn(string token)
        {
            _authToken = token;
            NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());
        }

        public void NotifyUserLoggedOut()
        {
            _authToken = string.Empty;
            NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());
        }
    }
}
