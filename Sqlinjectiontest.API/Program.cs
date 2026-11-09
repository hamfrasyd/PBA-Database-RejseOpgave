using Microsoft.Data.SqlClient;
using Sqlinjectiontest.API;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// The lab repository is used ONLY by the SQLInjectionTest controller.
builder.Services.AddScoped<ISQLInjectionTestRepository>(serviceProvider =>
{
    IConfiguration configuration = serviceProvider.GetRequiredService<IConfiguration>();
    IWebHostEnvironment environment = serviceProvider.GetRequiredService<IWebHostEnvironment>();

    if (environment.IsDevelopment() is false)
    {
        throw new InvalidOperationException("SQLInjectionTest is available only in Development.");
    }

    string connectionString = configuration.GetConnectionString("SQLInjectionTest")
        ?? throw new InvalidOperationException(
            "Missing ConnectionStrings:SQLInjectionTest in appsettings.Development.json.");

    // Do not allow the deliberately injectable query to run with SA/dbo credentials.
    SqlConnectionStringBuilder connectionSettings = new SqlConnectionStringBuilder(connectionString);
    if (string.Equals(connectionSettings.UserID, "SQLInjectionTestLabLogin", StringComparison.OrdinalIgnoreCase) is false ||
        string.Equals(connectionSettings.InitialCatalog, "TravelDb", StringComparison.OrdinalIgnoreCase) is false)
    {
        throw new InvalidOperationException(
            "SQLInjectionTest must connect to TravelDb using SQLInjectionTestLabLogin, never sa or an application login.");
    }

    string server = connectionSettings.DataSource;
    bool isLocalServer = string.Equals(server, "localhost,1433", StringComparison.OrdinalIgnoreCase) ||
        string.Equals(server, "127.0.0.1,1433", StringComparison.OrdinalIgnoreCase) ||
        string.Equals(server, ".,1433", StringComparison.OrdinalIgnoreCase) ||
        string.Equals(server, "(local),1433", StringComparison.OrdinalIgnoreCase);

    if (isLocalServer is false)
    {
        throw new InvalidOperationException(
            "SQLInjectionTest can only connect to a local SQL Server on port 1433.");
    }

    return new SQLInjectionTestRepository(connectionString);
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
else
{
    app.UseHttpsRedirection();

    // Hide the lab routes before controller activation (and repository resolution).
    app.Use(async (httpContext, nextMiddleware) =>
    {
        if (httpContext.Request.Path.StartsWithSegments("/api/sql-injection-test"))
        {
            httpContext.Response.StatusCode = StatusCodes.Status404NotFound;
            return;
        }

        await nextMiddleware(httpContext);
    });
}

app.MapControllers();

app.Run();
