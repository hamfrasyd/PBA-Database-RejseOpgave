using TravelApp.API.DatabaseSelection;
using TravelApp.BusinessLogic.DatabaseSelection;
using TravelApp.BusinessLogic.DependencyInjection;
using TravelApp.DataAccess.DependencyInjection;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

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

app.UseAuthorization();

app.MapControllers();

app.Run();
