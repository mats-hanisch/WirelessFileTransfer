using System.Diagnostics;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Primitives;


// create builder
var builder = WebApplication.CreateBuilder(args);

// extract config data from appsettings
int port = int.Parse(builder.Configuration["Port"] ?? "5000");
string uploadDir = builder.Configuration["FileUploadDirPath"] ?? @"C:\Uploads";
int maxFileSizeBytes = int.Parse(builder.Configuration["MaxFileSizeMB"] ?? "100") * 1000 * 1000; // convert from mb to bytes
string uiExePath = builder.Configuration["UIExePath"] ?? "UI.exe";
int uiExeMaxRuntimeHours = int.Parse(builder.Configuration["UIExeMaxRuntimeHours"] ?? "6");

// build app
var app = builder.Build();

// use default static files
app.UseDefaultFiles();
app.UseStaticFiles();

// fetch logger
var logger = app.Services.GetRequiredService<ILogger<Program>>();


// used to upload/post a file
app.MapPost("/upload-file", async (HttpRequest request) =>
{
    // check if the header Filename exists
    if (!request.Headers.TryGetValue("Filename", out var fileName))
    {
        // create error msg
        string errorMsg = "Header 'Filename' is missing!";

        // log error
        logger.LogError(errorMsg);

        // failed, respond with bad request
        return Results.BadRequest(errorMsg);
    }

    // validate length
    if (request.ContentLength is null || request.ContentLength > maxFileSizeBytes)
        return Results.BadRequest("File too large.");

    // make sure to strip of everything we don't need (and make it safe to use)
    fileName = Path.GetFileName(fileName);

    // get path of the file where it will be stored
    var filePath = Path.Combine(uploadDir, fileName.ToString());

    // make sure target dir exist
    Directory.CreateDirectory(uploadDir);

    // read file from request body and store it in file system
    await using var fileStream = new FileStream(filePath, FileMode.Create, FileAccess.Write);
    await request.Body.CopyToAsync(fileStream);

    // assemble info msg
    string infoMsg = $"File '{fileName}' saved successfully!";

    // log info
    logger.LogInformation(infoMsg);

    // success
    return Results.Ok(infoMsg);
});


// assemble api url
string apiUrl = $"http://0.0.0.0:{port}";

// start UI.exe to show user UI
if (!File.Exists(uiExePath))
{
    logger.LogWarning("UI executable not found at path: {Path}! Hosting api without GUI!", uiExePath);

    app.Run(apiUrl);
}
else
{
    // start api in the background
    var apiTask = app.RunAsync(apiUrl);
    try
    {
        var uiProcess = Process.Start(new ProcessStartInfo
        {
            FileName = uiExePath,
            Arguments = port.ToString(),
            UseShellExecute = true
        });
        logger.LogInformation("UI started successfully from path: {Path}", uiExePath);

        // wait till user closed UI win
        uiProcess?.WaitForExit(TimeSpan.FromHours(uiExeMaxRuntimeHours));

        // shutdown api
        await app.StopAsync();
    }
    catch (Exception ex)
    {
        logger.LogError(ex, "Failed to start UI executable at path: {Path}", uiExePath);
    }
}