using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Net.Http;
using System.Runtime.InteropServices;
using System.Security.Claims;
using BlazorApp1.Models;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using Newtonsoft.Json.Linq;

namespace BlazorApp1.Service
{
    public class AuthService
    {
        private readonly IJSRuntime _runtime;
        private readonly HttpClient _httpClient;
        private readonly NavigationManager _navigationManager;
        private readonly string _storageKey = "authToken";
        private readonly int _timeoutInMinutes = 10;

        public AuthService(IJSRuntime runtime, HttpClient httpClient, NavigationManager navigationManager)
        {
            _runtime = runtime;
            _httpClient = httpClient;
            _navigationManager = navigationManager;
        }

        public async Task<bool> Login(LoginModel loginModel)
        {
            var token = await GetTokenFromApi(loginModel);
            if (!string.IsNullOrEmpty(token))
            {
                await SetTokenWithTimeout(token);
                return true;
            }
            return false;
        }

        public async Task<bool> Logout()
        {
            await _runtime.InvokeVoidAsync("sessionStorage.clear");
            _navigationManager.NavigateTo("/login");
            return true;
        }

        private async Task SetTokenWithTimeout(string token)
        {
            var timeoutMilliseconds = _timeoutInMinutes * 60 * 1000;
            var expiration = DateTime.Now.AddMilliseconds(timeoutMilliseconds);

            await _runtime.InvokeVoidAsync("sessionStorage.setItem", _storageKey, token);
            await _runtime.InvokeVoidAsync("sessionStorage.setItem", $"{_storageKey}_expiration", expiration.ToString("O"));
        }

        public async Task<string> GetTokenAsync()
        {
            if (!RuntimeInformation.IsOSPlatform(OSPlatform.Create("Browser")))
            {
                return null;
            }

            return await _runtime.InvokeAsync<string>("sessionStorage.getItem", _storageKey);
        }

        public async Task<string> GetToken()
        {
            return await _runtime.InvokeAsync<string>("sessionStorage.getItem", _storageKey);
        }

        public async Task<bool> isAuthenticated()
        {
            var token = await GetToken();
            return !string.IsNullOrEmpty(token);
        }

        private async Task<string> GetTokenFromApi(LoginModel loginModel)
        {
            try
            {
                var response = await _httpClient.PostAsJsonAsync<LoginModel>("/api/Auth", loginModel);

                if (response.IsSuccessStatusCode)
                {
                    var token = await response.Content.ReadAsStringAsync();
                    return token;
                }
                else if (response.StatusCode == HttpStatusCode.BadRequest)
                {
                    return null;
                }
                else
                {
                    return null;
                }
            }
            catch (HttpRequestException)
            {
                return null;
            }
        }

    }
}
