using Cits.Api.Extensions;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.ConfigureNSwag();
builder.Services.ConfigureOpenTelemetry();



var app = builder.Build();

app.Logger.StartingApp(DateTime.Now);
app.RegisterApplicationLifeTimeEvents();

app.UseNSwag();

app.UseAuthorization();

app.MapControllers();



app.Run();
