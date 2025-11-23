using Services;
using Configuration.Extensions;
using Encryption.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddRazorPages();

builder.Configuration.AddSecrets(builder.Environment);
builder.Services.AddEncryptions(builder.Configuration);
builder.Services.AddVersionInfo();
builder.Services.AddEnvironmentInfo();

#region Setup the Dependency service
builder.Services.AddSingleton<IQuoteService, QuoteService>();
builder.Services.AddSingleton<LatinService>();
builder.Services.AddScoped<IAlbumsService, AlbumsServiceWapi>();
builder.Services.AddHttpClient(name: "MusicWebApi", configureClient: options =>
{
    options.BaseAddress = new Uri(builder.Configuration["DataService:WebApiBaseUri"]);
    options.DefaultRequestHeaders.Accept.Add(
        new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue(
            mediaType: "application/json",
            quality: 1.0));
});
#endregion

var app = builder.Build();

//using Hsts and https to secure the site
if (!app.Environment.IsDevelopment())
{
    //https://en.wikipedia.org/wiki/HTTP_Strict_Transport_Security
    //https://learn.microsoft.com/en-us/aspnet/core/security/enforcing-ssl
    app.UseHsts();
    app.UseDeveloperExceptionPage();
}

app.UseHttpsRedirection();

//Enable static and default files
app.UseDefaultFiles();
app.UseStaticFiles();

//Map Razorpages into Pages folder
app.MapRazorPages();


//Map aliases
app.MapGet("/lovequotes", () => Results.Redirect("~/Studies/Model/Search?search=Love"));
app.MapGet("/workquotes", () => Results.Redirect("~/Studies/Model/Search?search=work"));
app.MapGet("/quotes/microsoft", () => Results.Redirect("~/Studies/Model/Search?search=bill gates"));
app.MapGet("/hello", () =>
{
    //read the environment variable ASPNETCORE_ENVIRONMENT
    //Change in launchSettings.json, (not VS2022 Debug/Release)
    var _env = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
    var _envMyOwn = Environment.GetEnvironmentVariable("MyOwn");

    return $"Hello World!\nASPNETCORE_ENVIRONMENT: {_env}\nMyOwn: {_envMyOwn}";
});

app.Run();
