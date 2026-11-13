using Microsoft.AspNetCore.Authentication.Cookies;
using TravelApp.API.Auth;
using TravelApp.API.DatabaseSelection;
using TravelApp.BusinessLogic.Auth;
using TravelApp.BusinessLogic.DatabaseSelection;
using TravelApp.BusinessLogic.DependencyInjection;
using TravelApp.BusinessLogic.Exceptions.Filters;
using TravelApp.DataAccess.DependencyInjection;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers(options =>
{
    options.Filters.Add<TravelExceptionFilter>();
});
// Add Swagger
builder.Services.AddSwaggerGen(options =>
{
    string xmlFileName = $"{typeof(Program).Assembly.GetName().Name}.xml";
    string xmlFilePath = Path.Combine(AppContext.BaseDirectory, xmlFileName);

    options.IncludeXmlComments(xmlFilePath);
});

// Add CORS policy
// Taken from Configuration via fx. appsettings.json
// If the Section is missing, or the array is empty, no CORS policy will be added.
string[] allowedOrigins = builder.Configuration.GetSection("Cors:AllowedOrigins").Get<string[]>() ?? [];
if (allowedOrigins.Length > 0)
{
    builder.Services.AddCors(options => options.AddPolicy("TravelAppFrontend", policy =>
        policy.WithOrigins(allowedOrigins)
              .WithHeaders("Content-Type", HeaderName_For_DatabaseProvider.Key)
              .WithMethods("GET", "POST", "PUT", "PATCH", "DELETE")
              .AllowCredentials()));
}

// Register Application Services
// -- Add API Services
// --- Add AUTH services
builder.Services.AddScoped<ICurrentUser, CurrentUser>();
// Cookie session for the local demonstration login (no user passwords exist in dbo.User).
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.Cookie.Name = "TravelApp.Session";
        options.Cookie.HttpOnly = true;
        options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
        options.Cookie.SameSite = SameSiteMode.Strict;
        options.ExpireTimeSpan = TimeSpan.FromHours(8);
        options.Events.OnRedirectToLogin = context =>
        {
            context.Response.StatusCode = StatusCodes.Status401Unauthorized;
            return Task.CompletedTask;
        };
        options.Events.OnRedirectToAccessDenied = context =>
        {
            context.Response.StatusCode = StatusCodes.Status403Forbidden;
            return Task.CompletedTask;
        };
    });
builder.Services.AddAuthorization();


// -- Add Business Logic Services
builder.Services.AddBusinessLogic();

// -- Add Data Access Services
builder.Services.AddDataAccess(builder.Configuration);

var app = builder.Build();

// Use Swagger
app.UseSwagger();
app.UseSwaggerUI();


app.UseHttpsRedirection();

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
