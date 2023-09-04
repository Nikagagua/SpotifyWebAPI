using Spotify_WebAPI.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddHttpClient<ISpotifyAccountService, SpotifyAccountService>(client =>
{
    client.BaseAddress = new Uri("https://accounts.spotify.com");
});


builder.Services.AddHttpClient<ISpotifyService, SpotifyService>(c =>
{
    c.BaseAddress = new Uri("https://api.spotify.com/v1/");
    c.DefaultRequestHeaders.Add("Accept", "application/json");
});

builder.Services.AddControllersWithViews();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
}
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();