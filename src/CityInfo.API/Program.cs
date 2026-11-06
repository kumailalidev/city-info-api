using Asp.Versioning;

using CityInfo.API;
using CityInfo.API.Data;
using CityInfo.API.DbContexts;
using CityInfo.API.Services;

using Microsoft.AspNetCore.StaticFiles;
using Microsoft.EntityFrameworkCore;

using Scalar.AspNetCore;

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
// var apiVersions = new[] { "v1", "v2" };
// foreach (var apiVersion in apiVersions)
// {
//     builder.Services.AddOpenApi(apiVersion);
// }

// Register OpenApi services with multiple version support using string literals; required
// for detecting and reading generated document file
builder.Services.AddOpenApi("v1", options =>
{
    // Customizing OpenAPI doc.
    options.AddDocumentTransformer((document, context, cancellationToken) =>
    {
        document.Info = new()
        {
            Title = "City Info API",
            Version = context.DocumentName,
            Description = "Through this API you can access cities and their points of interest."
        };

        return Task.CompletedTask;
    });
});
builder.Services.AddOpenApi("v2", options =>
{
    // Customizing OpenAPI doc.
    options.AddDocumentTransformer((document, context, cancellationToken) =>
    {
        document.Info = new()
        {
            Title = "City Info API",
            Version = context.DocumentName,
            Description = "Through this API you can access cities and their points of interest."
        };

        return Task.CompletedTask;
    });
});

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

    app.MapScalarApiReference(options =>
    {
        options.WithTitle("City Info API")
         .WithTheme(ScalarTheme.Solarized)
         .WithDefaultHttpClient(ScalarTarget.CSharp, ScalarClient.HttpClient)
         .AddPreferredSecuritySchemes("Bearer");
    });
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();