using Microsoft.Extensions.Configuration;
using Serilog;
using Serilog.Events;
using System.Reflection;
using Terragate.Api;
using Terragate.Api.Services;

// Create web application builder.
var builder = WebApplication.CreateBuilder(args);

builder.Configuration.AddEnvironmentVariables();

// Create serilog logger.
var logger = new LoggerConfiguration()
    .ReadFrom.Configuration(builder.Configuration)
    .CreateLogger()
    .ForContext<Program>();

// Get application version.
var assembly = Assembly.GetExecutingAssembly();
var version = assembly.GetCustomAttribute<AssemblyInformationalVersionAttribute>()?.InformationalVersion;

// Log application version.
logger.Information($"Starting {assembly.GetName().Name} {{version}}", version);

// Use serilog for webhosting.
builder.Host.UseSerilog(logger);

// Add services to the container.
builder.Services.AddControllers();

// Add terraform services
builder.Services.AddTerraform(builder.Configuration);

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Add AutoMapper service
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{        
    logger.Warning($"Adding swagger http://[::]80/swagger/index.html");
    app.UseSwagger();
    app.UseSwaggerUI();

    // Show environment variables 
    if (logger.IsEnabled(LogEventLevel.Debug))
    {
        var variables = Environment.GetEnvironmentVariables();
        foreach (var key in variables.Keys.Cast<string>().Order())
            logger.ForContext(typeof(Environment)).Debug("{key:l}={value:l}", key, variables[key]);
    }
}

app.UseTerraform();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
