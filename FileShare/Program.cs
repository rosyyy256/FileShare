using FileShare;
using FileShare.Configuration;
using FileShare.Services;
using MongoDB.Bson;
using MudBlazor.Services;

var builder = WebApplication.CreateBuilder(args);
var services = builder.Services;
var configuration = builder.Configuration;

services.AddRazorPages();
services.AddServerSideBlazor();
services.AddOptions<MongoSettings>()
    .Bind(configuration.GetSection(MongoSettings.CONFIG_SECTION_NAME))
    .ValidateDataAnnotations()
    .ValidateOnStart();
services.AddSingleton<ApplicationContext>();
services.AddSingleton<DisposableDictionary<string>>();
services.AddMudServices();

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
}

app.UseStaticFiles();

app.UseRouting();

app.MapGet("/file/{id}", async (ApplicationContext db, HttpContext context, string id) =>
{
    await db.Bucket.DownloadToStreamAsync(new ObjectId(id), context.Response.Body);
});

app.MapGet("/disposable/{id}", async (ApplicationContext db, DisposableDictionary<string> linkService, HttpContext context, string id) =>
{
    try
    {
        var fileId = linkService.GetAndDispose(id);
        await db.Bucket.DownloadToStreamAsync(new ObjectId(fileId), context.Response.Body);
        return Results.Ok();
    }
    catch (KeyNotFoundException)
    {
        return Results.NotFound();
    }
});

app.MapBlazorHub();
app.MapFallbackToPage("/_Host");

app.Run();