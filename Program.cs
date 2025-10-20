using Serilog;
using Serilog.Events;

var builder = WebApplication.CreateBuilder(args);

// Configure Serilog

Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Override("Microsoft", LogEventLevel.Warning) // Suppress Microsoft logs
    .MinimumLevel.Override("System", LogEventLevel.Warning)    // Suppress System logs
    .MinimumLevel.Information()                                // Allow your logs
    .WriteTo.Console()
    .WriteTo.File(
        path: "Logger/log-.txt",
        rollingInterval: RollingInterval.Day,
        outputTemplate: "{Timestamp:yyyy-MM-dd HH:mm:ss} [{Level:u3}] {Message:lj}{NewLine}{Exception}"
    )
    .CreateLogger();


builder.Host.UseSerilog(); // Use Serilog as the logging provider

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment() || app.Environment.IsProduction())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

//app.UseHttpsRedirection();
builder.WebHost.ConfigureKestrel(options =>
{
    options.ListenAnyIP(8080); // Match the OpenShift service port
});
app.UseAuthorization();
app.MapControllers();
app.Run();