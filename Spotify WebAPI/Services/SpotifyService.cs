using Spotify_WebAPI.Models;
using System.Net.Http.Headers;
using System.Text.Json;

namespace Spotify_WebAPI.Services
{
    public class SpotifyService : ISpotifyService
    {
        private readonly HttpClient _httpClient;

        public SpotifyService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<IEnumerable<Release>> GetNewReleases(string countryCode, int limit, string accessToken)
        {
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

            // Use await here to get the response
            var response = await _httpClient.GetAsync($"https://api.spotify.com/v1/browse/new-releases?country={countryCode}&limit={limit}");

            // Check if the response is successful
            response.EnsureSuccessStatusCode();

            // Get the response content
            var responseContent = await response.Content.ReadAsStringAsync();

            // Deserialize the response content
            var responseObject = JsonSerializer.Deserialize<GetNewRelease>(responseContent);

            return responseObject?.albums?.items?.Select(item => new Release
            {
                Name = item.name,
                Date = item.release_date,
                ImageUrl = item.images?.FirstOrDefault()?.url,
                Link = item.external_urls?.spotify,
                Artists = string.Join(", ", item.artists.Select(artist => artist.name))
            });
        }
    }
}