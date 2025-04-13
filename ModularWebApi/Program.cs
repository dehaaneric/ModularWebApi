using YourApp.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

// Configure plugin manager
var pluginManager = new PluginManager(
    builder.Services.BuildServiceProvider().GetRequiredService<ILogger<PluginManager>>());

// Discover plugins
//var pluginsPath = Path.Combine(AppContext.BaseDirectory, "plugins");
pluginManager.DiscoverPlugins(@"C:\Source\ModularWebApi\PluginSource");

// Configure plugin services
foreach (var plugin in pluginManager.Plugins)
{
    plugin.ConfigureServices(builder.Services, builder.Configuration);
}

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();
app.UseRouting();

// Configure plugin endpoints
app.UseEndpoints(endpoints =>
{
    foreach (var plugin in pluginManager.Plugins)
    {
        plugin.ConfigureEndpoints(endpoints);
    }
});

//app.MapGet("/weatherforecast", () =>
//{
//})
//.WithName("GetWeatherForecast");

app.Run();
