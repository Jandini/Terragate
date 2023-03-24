using AutoMapper;
using Microsoft.OpenApi.Models;
using Serilog;
using Serilog.Events;
using Serilog.Sinks.Elasticsearch;
using System.Reflection;
using System.Text.RegularExpressions;
using Terragate.Api;
using Terragate.Api.Services;

// Create web application builder
var builder = WebApplication.CreateBuilder(args);

// Alter configuration with environment variables
builder.Configuration.AddEnvironmentVariables();

// Get application settings
var appSettings = builder.Configuration.Get<AppSettings>()!;
builder.Services.AddSingleton(appSettings);


// Application name and version can be overridden by APPLICATION_NAME and APPLICATION_VERSION environment variables.
var appName = appSettings.ApplicationName ?? builder.Environment.ApplicationName;
var appVersion = Assembly.GetExecutingAssembly().GetCustomAttribute<AssemblyInformationalVersionAttribute>()?.InformationalVersion;

var elasticOptions = new ElasticsearchSinkOptions(appSettings.ElasticsearchUri)
{
    // Elasticsearch index format must not be longer than 255 character. 
    // https://www.elastic.co/guide/en/elasticsearch/reference/current/indices-create-index.html
    IndexFormat = Regex.Replace($"{appName}-logs-{builder.Environment.EnvironmentName}-{DateTime.UtcNow:yyyy-MM}".ToLower(), "[\\\\/\\*\\?\"<>\\|# ]", "-"),
    AutoRegisterTemplate = true,
    // Set environemnt variable ELASTICSEARCH_DEBUG=true do debug elasticsearch logging
    ModifyConnectionSettings = !appSettings.ElasticsearchDebug ? null : config => config.OnRequestCompleted(d => Console.WriteLine(d.DebugInformation)) 
};


// Create serilog logger
var logger = new LoggerConfiguration()
    .WriteTo.Elasticsearch(elasticOptions)
    .Enrich.WithProperty("Environment", builder.Environment.EnvironmentName)
    .Enrich.WithProperty("Application", appName)
    .ReadFrom.Configuration(builder.Configuration)
    .CreateLogger()
    .ForContext<Program>();


logger.Information("Starting {appName} {appVersion}", appName, appVersion);

if (appSettings.ElasticsearchUri != null)
{
    logger.Information("Elasticsearch logging: {elasticsearchUri}", appSettings.ElasticsearchUri);
    logger.Information("Elasticsearch pattern: {indexFormat}", elasticOptions.IndexFormat);

    if (elasticOptions.IndexFormat.Length > 255)
        throw new Exception("Elasticsearch index format exceeds 255 characters.");
}
else
    logger.Warning("Elasticsearch logging not configured");

// Use serilog for web hosting
builder.Host.UseSerilog(logger);

// Add services to the container
builder.Services.AddControllers()
    // Suppress ProblemDetails schema
    .ConfigureApiBehaviorOptions(o => o.SuppressMapClientErrors = true);

// Add terraform services
builder.Services.AddTerraform(builder.Configuration);

// Add health service
builder.Services.AddHealth();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen(
    // Get rid of Dto postfix 
    options => {
        options.CustomSchemaIds((type) => type.Name.EndsWith("Dto")
            ? type.Name[..^3]
            : type.Name);

        options.SwaggerDoc("v1", new OpenApiInfo()
        {
            Title = appName
        });        
    });

// Add AutoMapper service
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

// Add HttpClient service
builder.Services.AddHttpClient();

var app = builder.Build();


#if (DEBUG)
// Assert mapper configuration only in DEBUG 
app.Services.GetRequiredService<IMapper>().ConfigurationProvider.AssertConfigurationIsValid();
#endif

// Configure the HTTP request pipeline
if (app.Environment.IsDevelopment())
{        
    logger.Warning($"Adding swagger UI");
    app.UseSwagger();
    app.UseSwaggerUI( options => options.DocumentTitle = appName);

    // Show environment variables 
    if (logger.IsEnabled(LogEventLevel.Debug))
    {
        var variables = Environment.GetEnvironmentVariables();
        foreach (var key in variables.Keys.Cast<string>().Order())
            logger.ForContext(typeof(Environment)).Debug("{key}={value}", key, variables[key]);
    }
}

app.UseMiddleware<ExceptionMiddleware>();

app.UseTerraform();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
