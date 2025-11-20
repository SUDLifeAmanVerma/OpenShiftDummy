using Serilog;
using Serilog.Events;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

// Configure Serilog

// Choose a path inside a mounted volume or /tmp if no PVC (non-persistent)
//var logDirectory = Environment.GetEnvironmentVariable("LOG_DIR")
//                  ?? "/var/log/myapp"; // recommended PVC mount path
//Directory.CreateDirectory(logDirectory); // ensure it exists

//var logFilePath = Path.Combine(logDirectory, "log-.txt");

//Log.Logger = new LoggerConfiguration()
//    .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
//    .MinimumLevel.Override("System", LogEventLevel.Warning)
//    .MinimumLevel.Information()
//    .WriteTo.Console(
//        outputTemplate: "{Timestamp:yyyy-MM-dd HH:mm:ss} [{Level:u3}] {Message:lj}{NewLine}{Exception}"
//    )
//    .WriteTo.File(
//        path: logFilePath,
//        rollingInterval: RollingInterval.Day,
//        retainedFileCountLimit: 14, // avoid unbounded growth
//        shared: true,
//        outputTemplate: "{Timestamp:yyyy-MM-dd HH:mm:ss} [{Level:u3}] {Message:lj}{NewLine}{Exception}"
//    )
//    .CreateLogger();

//builder.Host.UseSerilog();

var logDir = Path.Combine("/tmp", "Logger");
Directory.CreateDirectory(logDir);

Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
    .MinimumLevel.Override("System", LogEventLevel.Warning)
    .MinimumLevel.Information()
    .WriteTo.Console(
        outputTemplate: "{Timestamp:yyyy-MM-dd HH:mm:ss} [{Level:u3}] {Message:lj}{NewLine}{Exception}"
    )
    .WriteTo.File(
        path: Path.Combine(logDir, "log-.txt"), // <-- no leading slash
        rollingInterval: RollingInterval.Day,
        retainedFileCountLimit: 14,              // avoid unbounded growth
        shared: true,
        outputTemplate: "{Timestamp:yyyy-MM-dd HH:mm:ss} [{Level:u3}] {Message:lj}{NewLine}{Exception}"
    )
    .CreateLogger();

builder.Host.UseSerilog();

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