using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace Microsoft.Extensions.Hosting;

public static class NSwagExtension
{
    public static IServiceCollection ConfigureNSwag(this IServiceCollection services)
    {
        services.AddOpenApiDocument();
        return services;
    }

    public static WebApplication UseNSwag(this WebApplication app)
    {
        if (app.Environment.IsDevelopment())
        {
            // Add OpenAPI 3.0 document serving middleware
            // Available at: http://localhost:<port>/swagger/v1/swagger.json
            app.UseOpenApi();

            // Add web UIs to interact with the document
            // Available at: http://localhost:<port>/swagger
            app.UseSwaggerUi();
        }

        return app;
    }
}
