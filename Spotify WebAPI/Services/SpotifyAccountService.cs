using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Spotify_WebAPI.Models;

namespace Spotify_WebAPI.Services
{
    public class SpotifyAccountService : ISpotifyAccountService
    {
        private readonly HttpClient _httpClient;

        public SpotifyAccountService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<string> GetToken(string clientId, string clientSecret)
        {
            var request = new HttpRequestMessage(HttpMethod.Post, "https://accounts.spotify.com/api/token");

            request.Headers.Authorization = new AuthenticationHeaderValue("Basic", Convert.ToBase64String(Encoding.UTF8.GetBytes($"{clientId}:{clientSecret}")));

            request.Content = new FormUrlEncodedContent(new Dictionary<string, string>
            {
                { "grant_type", "client_credentials"}
            });

            try
            {
                var response = await _httpClient.SendAsync(request);

                if (response.StatusCode != HttpStatusCode.OK)
                {
                    Debug.WriteLine($"Error getting token: {response.StatusCode} {response.ReasonPhrase}");
                    return null;
                }

                var responseContent = await response.Content.ReadAsStringAsync();
                Debug.WriteLine(responseContent);

                var authResult = JsonSerializer.Deserialize<AuthResult>(responseContent);
                if (authResult == null)
                {
                    Debug.WriteLine("Failed to deserialize response content");
                    return null;
                }

                return authResult.AccessToken;
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Exception getting token: {ex.Message}");
                return null;
            }
        }
    }
}
