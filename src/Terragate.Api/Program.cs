using AutoMapper;
using Serilog;
using Serilog.Events;
using Terragate.Api.Services;

// Create web application builder
var builder = WebApplication.CreateBuilder(args);

// Alter configuration with environment variables
builder.Configuration.AddEnvironmentVariables();

// Create serilog logger
var logger = new LoggerConfiguration()
    .ReadFrom.Configuration(builder.Configuration)
    .CreateLogger()
    .ForContext<Program>();

// Log application version
var health = new HealthService().GetHealthInfo();
logger.Information($"Starting {health.ServiceName} {{version:l}}", health.ServiceVersion);

// Use serilog for web hosting
builder.Host.UseSerilog(logger);

// Add services to the container
builder.Services.AddControllers();

// Add terraform services
builder.Services.AddTerraform(builder.Configuration);

// Add health service
builder.Services.AddHealth();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen(
    // Get rid of Dto postfix 
    options => options.CustomSchemaIds((type) => type.Name.EndsWith("Dto") 
        ? type.Name[..^3] 
        : type.Name));

// Add AutoMapper service
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

var app = builder.Build();


#if (DEBUG)
// Assert mapper configuration only in DEBUG 
app.Services.GetRequiredService<IMapper>().ConfigurationProvider.AssertConfigurationIsValid();
#endif

// Configure the HTTP request pipeline
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
