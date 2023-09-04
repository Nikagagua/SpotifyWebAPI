namespace Spotify_WebAPI.Services
{
    public interface ISpotifyAccountService
    {
        Task<string> GetToken(string clientId, string clientSecret);
    }
}