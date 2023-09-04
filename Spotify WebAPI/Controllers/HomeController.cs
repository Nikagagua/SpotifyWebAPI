using Microsoft.AspNetCore.Mvc;
using Spotify_WebAPI.Models;
using Spotify_WebAPI.Services;
using System.Diagnostics;

namespace Spotify_WebAPI.Controllers
{
    public class HomeController : Controller
    {
        private readonly ISpotifyAccountService _spotifyAccountService;

        private readonly ISpotifyService _spotifyService;
        private readonly IConfiguration _configuration;

        public HomeController(ISpotifyAccountService spotifyAccountService, IConfiguration configuration, ISpotifyService spotifyService)
        {
            _spotifyAccountService = spotifyAccountService;
            _spotifyService = spotifyService;
            _configuration = configuration;
        }

        public async Task<IActionResult> Index()
        {
            var newReleases = await GetNewRelease();

            return View(newReleases);
        }

        private async Task<IEnumerable<Release>> GetNewRelease()
        {
            var clientId = _configuration["Spotify:ClientId"] ?? string.Empty;
            var clientSecret = _configuration["Spotify:ClientSecret"] ?? string.Empty;

            try
            {
                var token = await _spotifyAccountService.GetToken(clientId, clientSecret);
                var newReleases = await _spotifyService.GetNewReleases("US", 20, token);

                return newReleases;
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error: {ex.Message}");

                // Return an empty collection in case of an exception
                return Enumerable.Empty<Release>();
            }
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public IActionResult AboutMe()
        {
            return View();
        }
    }
}