using Microsoft.OpenApi;

namespace pitaneAPI.Profiles
{
        public static class OpenApiExtensions
        {
            public static IServiceCollection AddOpenApiConfigured(this IServiceCollection services)
            {
                services.AddOpenApi(options =>
                {
                    options.AddDocumentTransformer((document, context, CancellationToken) =>
                    {
                        document.Components ??= new();
                        document.Components.SecuritySchemes = new Dictionary<string, IOpenApiSecurityScheme>
                        {
                            ["Bearer"] = new OpenApiSecurityScheme
                            {
                                Type = SecuritySchemeType.Http,
                                Scheme = "bearer",
                                Description = "Enter JWT Token in the format: Bearer {your token}",
                                Name = "Authorization",
                                BearerFormat = "JWT",
                            }
                        };
                        document.Security = new[]
                        {
                        new OpenApiSecurityRequirement
                        {
                            { new OpenApiSecuritySchemeReference("Bearer"), new List<string>() }
                        }
                    };
                        return Task.CompletedTask;
                    });
                });

                return services;
            }
        }
}
