using Serilog;
using System.Reflection;
using Terragate.Api.Services;

// Create web application builder.
var builder = WebApplication.CreateBuilder(args);

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
builder.Services.AddScoped<ITerraformProcessService, TerraformProcessService>();
builder.Services.AddScoped<ITerraformDeploymentRepository, TerraformDeploymentRepository>();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{    
    
    logger.Warning($"Adding swagger http://[::]80/swagger/index.html");
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
