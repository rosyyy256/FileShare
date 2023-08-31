using FileShare;
using FileShare.Configuration;
using FileShare.Services;
using MongoDB.Bson;
using MudBlazor;
using MudBlazor.Services;

var builder = WebApplication.CreateBuilder(args);
var services = builder.Services;
var configuration = builder.Configuration;

services.AddRazorPages();
services.AddServerSideBlazor();
services.AddOptions<ApplicationSettings>()
    .Bind(configuration);
services.AddOptions<MongoSettings>()
    .Bind(configuration.GetSection(MongoSettings.CONFIG_SECTION_NAME))
    .ValidateDataAnnotations()
    .ValidateOnStart();
services.AddSingleton<FilesContext>();
services.AddSingleton<ThrowawayDictionary<ObjectId>>();
services.AddMudServices(config =>
{
    config.SnackbarConfiguration.PositionClass = Defaults.Classes.Position.BottomRight;
});

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
}

app.UseStaticFiles();

app.UseRouting();

app.MapGet("/file/{id}", async (FilesContext db, ThrowawayDictionary<ObjectId> throwawayDict, string id) =>
{
    if ((ObjectId.TryParse(id, out var objectId) || throwawayDict.TryGetAndThrowaway(id, out objectId)) && await db.FileExistsAsync(objectId))
    {
        var fileStream = await db.OpenDownloadStreamAsync(objectId);
        var fileInfo = await db.GetFileInfoAsync(objectId);
        return Results.File(fileStream, fileDownloadName: fileInfo.Filename);
    }
    else
    {
        return Results.NotFound();
    }
});

app.MapBlazorHub();
app.MapFallbackToPage("/_Host");

app.Run();