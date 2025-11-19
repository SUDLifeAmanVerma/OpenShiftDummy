using Serilog;
using Serilog.Events;

var builder = WebApplication.CreateBuilder(args);

// Configure Serilog

// Choose a path inside a mounted volume or /tmp if no PVC (non-persistent)
var logDirectory = Environment.GetEnvironmentVariable("LOG_DIR")
                  ?? "/var/log/myapp"; // recommended PVC mount path
Directory.CreateDirectory(logDirectory); // ensure it exists

var logFilePath = Path.Combine(logDirectory, "log-.txt");

Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
    .MinimumLevel.Override("System", LogEventLevel.Warning)
    .MinimumLevel.Information()
    .WriteTo.Console(
        outputTemplate: "{Timestamp:yyyy-MM-dd HH:mm:ss} [{Level:u3}] {Message:lj}{NewLine}{Exception}"
    )
    .WriteTo.File(
        path: logFilePath,
        rollingInterval: RollingInterval.Day,
        retainedFileCountLimit: 14, // avoid unbounded growth
        shared: true,
        outputTemplate: "{Timestamp:yyyy-MM-dd HH:mm:ss} [{Level:u3}] {Message:lj}{NewLine}{Exception}"
    )
    .CreateLogger();

builder.Host.UseSerilog();




//Log.Logger = new LoggerConfiguration()
//    .MinimumLevel.Override("Microsoft", LogEventLevel.Warning) // Suppress Microsoft logs
//    .MinimumLevel.Override("System", LogEventLevel.Warning)    // Suppress System logs
//    .MinimumLevel.Information()                                // Allow your logs
//    .WriteTo.Console()
//    .WriteTo.File(
//        path: "Logger/log-.txt",
//        rollingInterval: RollingInterval.Day,
//        outputTemplate: "{Timestamp:yyyy-MM-dd HH:mm:ss} [{Level:u3}] {Message:lj}{NewLine}{Exception}"
//    )
//    .CreateLogger();


//builder.Host.UseSerilog(); // Use Serilog as the logging provider

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.WebHost.ConfigureKestrel(options =>
{
    options.ListenAnyIP(8080); // Match the OpenShift service port
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment() || app.Environment.IsProduction() || app.Environment.IsStaging())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

//app.UseHttpsRedirection();

app.UseAuthorization();
app.MapControllers();
app.Run();