using TravelDb.Core.Handlers;
using TravelDb.Core.Interfaces;
using TravelDb.Infrastructure.GraphRepos;
using TravelDb.Infrastructure.MongoRepos;
using TravelDb.Infrastructure.SQLRepos;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddScoped<ExampleHandler>();
builder.Services.AddScoped<GraphRepository>();
builder.Services.AddScoped<MongoRepository>();
builder.Services.AddScoped<SQLRepository>();

builder.Services.AddScoped<IRepository>(serviceProvider =>
{
    string? provider = builder.Configuration["Database:Provider"]?.Trim().ToLowerInvariant();

    switch (provider)
    {
        case "graph":
            return serviceProvider.GetRequiredService<GraphRepository>();

        case "mongo":
            return serviceProvider.GetRequiredService<MongoRepository>();

        case "sql":
            return serviceProvider.GetRequiredService<SQLRepository>();

        default:
            throw new InvalidOperationException($"Invalid or missing database provider '{provider}'. Set Database:Provider to Graph, Mongo, or SQL.");
    }
});

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
