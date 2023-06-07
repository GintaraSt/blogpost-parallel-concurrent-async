var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

app.MapGet("/slow-response", async () =>
{
    await Task.Delay(5_000); // Wait for 5 seconds before returning.
    return "";
});

app.Run();