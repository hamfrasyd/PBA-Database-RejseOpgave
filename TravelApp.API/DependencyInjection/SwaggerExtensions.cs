using Microsoft.OpenApi;

namespace TravelApp.API.DependencyInjection
{
    /// <summary>Configures Swagger documentation and the interactive API client.</summary>
    public static class SwaggerExtensions
    {
        /// <summary>Registers Swagger with XML documentation and JWT bearer support.</summary>
        public static void AddTravelAppSwagger(this IServiceCollection services)
        {
            services.AddSwaggerGen(options =>
            {
                string xmlFileName = $"{typeof(Program).Assembly.GetName().Name}.xml";
                string xmlFilePath = Path.Combine(AppContext.BaseDirectory, xmlFileName);

                options.IncludeXmlComments(xmlFilePath);

                options.AddSecurityDefinition("bearer", new OpenApiSecurityScheme
                {
                    Type = SecuritySchemeType.Http,
                    Scheme = "bearer",
                    BearerFormat = "JWT"
                });

                options.AddSecurityRequirement(document => new OpenApiSecurityRequirement
                {
                    [new OpenApiSecuritySchemeReference("bearer", document)] = []
                });
            });
        }

        /// <summary>Enables Swagger and reuses the login token in Development.</summary>
        public static void UseTravelAppSwagger(this WebApplication app)
        {
            app.UseSwagger();
            app.UseSwaggerUI(options =>
            {
                if (!app.Environment.IsDevelopment())
                    return;

                options.UseResponseInterceptor("""
                    (response) => {
                        const url = new URL(response.url, window.location.href);

                        if (url.origin === window.location.origin &&
                            url.pathname === "/api/auth/login" &&
                            response.status === 200 &&
                            typeof response.body?.accessToken === "string") {
                            window.travelAppAccessToken = response.body.accessToken;
                        }

                        return response;
                    }
                    """);

                options.UseRequestInterceptor("""
                    (request) => {
                        const url = new URL(request.url, window.location.href);

                        if (url.origin === window.location.origin &&
                            url.pathname === "/api/auth/login") {
                            delete window.travelAppAccessToken;
                        } else if (url.origin === window.location.origin &&
                                   url.pathname.startsWith("/api/") &&
                                   window.travelAppAccessToken) {
                            request.headers.Authorization =
                                `Bearer ${window.travelAppAccessToken}`;
                        }

                        return request;
                    }
                    """);
            });
        }
    }
}