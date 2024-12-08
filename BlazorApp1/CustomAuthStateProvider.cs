using System.Net.Http;
using System.Security.Claims;
using System.Text.Json;
using BlazorApp1.Models;
using BlazorApp1.Service;
using Microsoft.AspNetCore.Components.Authorization;

namespace BlazorApp1
{
    public class CustomAuthStateProvider : AuthenticationStateProvider
    {
        private readonly AuthService _authService;
		private readonly HttpClient _httpClient;

		public CustomAuthStateProvider( AuthService authService, HttpClient httpClient)
        {
            _authService = authService;
            _httpClient = httpClient;
        }

        public override async Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            string token = await _authService.GetTokenAsync();
            if (string.IsNullOrEmpty(token))
            {
                var emptyIdentity = new ClaimsIdentity();
                var emptyUser = new ClaimsPrincipal(emptyIdentity);
                var emptyState = new AuthenticationState(emptyUser);
                return emptyState;
            }

            var identity = new ClaimsIdentity(ParseClaimFromJwt(token), "jwt");
            var user = new ClaimsPrincipal(identity);
            var state = new AuthenticationState(user);

            NotifyAuthenticationStateChanged(Task.FromResult(state));
            return state;
        }

        public static IEnumerable<Claim> ParseClaimFromJwt(string token)
        {
            var payload = token.Split('.')[1];
            var JsonBytes = ParseBase64WithoutPadding(payload);
            var keyValuePairs = JsonSerializer.Deserialize<Dictionary<string, object>>(JsonBytes);
            return keyValuePairs.Select(kvp => new Claim(kvp.Key, kvp.Value.ToString()));
        }

        public async Task<bool> UpdateAuthenticationStatus()
        {
			var token = await _authService.GetToken();
			var identity = new ClaimsIdentity(ParseClaimFromJwt(token), "jwt");
            var user = new ClaimsPrincipal(identity);
            var state = new AuthenticationState(user);

            NotifyAuthenticationStateChanged(Task.FromResult(state));
            return true;
        }

        private static byte[] ParseBase64WithoutPadding(string base64)
        {
            switch (base64.Length %4)
            {
                case 2: base64 += "=="; break;
                case 3: base64 += "="; break;
            }
            return Convert.FromBase64String(base64);
        }
    }
}
