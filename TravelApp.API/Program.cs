using Scalar.AspNetCore;
using TravelApp.API.DatabaseSelection;
using TravelApp.API.DependencyInjection;
using TravelApp.BusinessLogic.DatabaseSelection;
using TravelApp.BusinessLogic.DependencyInjection;
using TravelApp.DataAccess.DependencyInjection;
using TravelApp.API.ExceptionFilters;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddHttpContextAccessor();

// Add controllers and exception filters
builder.Services.AddControllers(options =>
{
    options.Filters.Add<TravelExceptionFilter>();
});

// Microsoft OpenAPI with JWT security
builder.Services.AddTravelAppOpenApi();

// Add CORS policy
// Taken from Configuration via fx. appsettings.json
// If the Section is missing, or the array is empty, no CORS policy will be added.
string[] allowedOrigins = builder.Configuration.GetSection("Cors:AllowedOrigins").Get<string[]>() ?? [];
if (allowedOrigins.Length > 0)
{
    builder.Services.AddCors(options =>
    options.AddPolicy("TravelAppFrontend", policy =>
        policy.WithOrigins(allowedOrigins)
               .WithHeaders("Content-Type", "Authorization", HeaderName_For_DatabaseProvider.Key)
                .WithMethods("GET", "POST", "PUT","PATCH", "DELETE")));
}

// Register Application Services

// 1) Add API Services
//  a) Add AUTH services
builder.AddTravelAppAuthentication();

// 2) Add Business Logic Services
builder.Services.AddBusinessLogic();

// 3) Add Data Access Services
builder.Services.AddDataAccess(builder.Configuration);

var app = builder.Build();

app.UseHttpsRedirection();

// OpenAPI and Scalar (Development only)
if (app.Environment.IsDevelopment())
{
    // Serve Scalar's custom JavaScript configuration.
    app.UseStaticFiles();

    // Generate and expose OpenAPI documentation.
    app.MapOpenApi();

    // Configure Scalar with automatic JWT handling.
    app.MapScalarApiReference(options =>
    {
        options.WithJavaScriptConfiguration( "/scalar/config.js");
    });
}

app.UseRouting();

// Use CORS policy
if (allowedOrigins.Length > 0)
{
    app.UseCors("TravelAppFrontend");
}

// Use DatabaseProviderMiddleware to select the appropriate database provider based on the request
app.UseMiddleware<DatabaseProviderMiddleware>();

// Use Authentication and Authorization. Authenticaton must be before Authorization, otherwise the user will not be authenticated and will always get a 403 Forbidden response.
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();