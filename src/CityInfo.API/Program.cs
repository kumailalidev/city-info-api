using Asp.Versioning;

using CityInfo.API;
using CityInfo.API.Data;
using CityInfo.API.DbContexts;
using CityInfo.API.Services;

using Microsoft.AspNetCore.StaticFiles;
using Microsoft.EntityFrameworkCore;

using Serilog;

// Serilog configuration
Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Debug()
    .WriteTo.Console()
    .WriteTo.File("logs/cityinfo.txt", rollingInterval: RollingInterval.Day)
    .CreateLogger();

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseSerilog();

// Add services to the container.
builder.Services.AddControllers()
    .AddXmlDataContractSerializerFormatters();


builder.Services.AddProblemDetails();

// Register OpenApi services with multiple version support
var apiVersions = new[] { "v1", "v2" };
foreach (var apiVersion in apiVersions)
{
    builder.Services.AddOpenApi(apiVersion);
}


builder.Services.AddSingleton<FileExtensionContentTypeProvider>();

builder.Services.AddSingleton<CitiesDataStore>();

builder.Services.AddDbContext<CityInfoContext>(dbContextOptions =>
    dbContextOptions.UseSqlite(builder.Configuration["ConnectionStrings:CityInfoDBConnectionString"]
        ?? throw new InvalidOperationException()));

builder.Services.AddScoped<ICityInfoRepository, CityInfoRepository>();

builder.Services.AddAutoMapper(config => { },
    AppDomain.CurrentDomain.GetAssemblies()); // Use assemblies for auto. scan profiles

builder.Services.AddScoped<IPointOfInterestService, PointOfInterestService>();

#if DEBUG
builder.Services.AddTransient<IMailService, LocalMailService>();
#else
builder.Services.AddTransient<IMailService, CloudMailService>();
#endif

builder.Services.AddApiVersioning(setupAction =>
{
    setupAction.ReportApiVersions = true;
    setupAction.AssumeDefaultVersionWhenUnspecified = true;
    setupAction.DefaultApiVersion = new ApiVersion(1, 0);
})
.AddMvc()
.AddApiExplorer(setupAction =>
{
    setupAction.SubstituteApiVersionInUrl = true;
    setupAction.GroupNameFormat = "'v'V";
});

var app = builder.Build();

// Configure the HTTP request pipeline.

// add exception handler middleware
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler();
}

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi("/openapi/{documentName}.json");
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();