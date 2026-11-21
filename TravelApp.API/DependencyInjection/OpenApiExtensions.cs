
using Microsoft.AspNetCore.Authorization;
using Microsoft.OpenApi;

namespace TravelApp.API.DependencyInjection
{
    /// <summary>Configures OpenAPI documentation and JWT authentication.</summary>
    public static class OpenApiExtensions
    {
        public static void AddTravelAppOpenApi(this IServiceCollection services)
        {
            services.AddOpenApi(options =>
            {
                // Define the JWT bearer authentication scheme.
                options.AddDocumentTransformer((document, context, cancellationToken) =>
                {
                    document.Components ??= new OpenApiComponents();
                    document.Components.SecuritySchemes ??= new Dictionary<string, IOpenApiSecurityScheme>();

                    document.Components.SecuritySchemes["bearer"] = new OpenApiSecurityScheme
                    {
                        Type = SecuritySchemeType.Http,
                        Scheme = "bearer",
                        BearerFormat = "JWT"
                    };

                    return Task.CompletedTask;
                });

                // Mark endpoints that require JWT authentication.
                options.AddOperationTransformer((operation, context, cancellationToken) =>
                {
                    var endpointMetadata = context.Description.ActionDescriptor.EndpointMetadata;

                    bool allowsAnonymous = endpointMetadata.OfType<IAllowAnonymous>().Any();
                    bool requiresAuthorization = endpointMetadata.OfType<IAuthorizeData>().Any();

                    if (allowsAnonymous || !requiresAuthorization)
                    {
                        return Task.CompletedTask;
                    }

                    operation.Security ??= [];

                    operation.Security.Add(new OpenApiSecurityRequirement
                    {
                        [new OpenApiSecuritySchemeReference("bearer", context.Document)] = []
                    });

                    return Task.CompletedTask;
                });
            });
        }
    }
}
