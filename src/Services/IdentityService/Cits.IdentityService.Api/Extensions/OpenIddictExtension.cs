using Cits.IdentityService.Api.Workers;
using Cits.IdentityService.Domain.EntityFramework;
using Microsoft.EntityFrameworkCore;

namespace Microsoft.Extensions.Hosting;

public static class OpenIddictExtension
{
    /// <summary>
    /// 配置
    /// </summary>
    /// <param name="builder"></param>
    /// <returns></returns>
    public static IHostApplicationBuilder ConfigureOpenIddict(this IHostApplicationBuilder builder)
    {
        builder.Services.AddDbContext<OpenIddictDbContext>(options =>
        {
            // Configure Entity Framework Core to use Sqlite.
            options.UseSqlite(@"Data Source=freedb.db");

            // Register the entity sets needed by OpenIddict.
            // Note: use the generic overload if you need to replace the default OpenIddict entities.
            options.UseOpenIddict();
        });

        builder.Services.AddOpenIddict()

            // Register the OpenIddict core components.
            .AddCore(options =>
            {
                // Configure OpenIddict to use the Entity Framework Core stores and models.
                // Note: call ReplaceDefaultEntities() to replace the default entities.
                options.UseEntityFrameworkCore()
                       .UseDbContext<OpenIddictDbContext>();
            })

            // Register the OpenIddict server components.
            .AddServer(options =>
            {
                // Enable the token endpoint.
                options.SetTokenEndpointUris("connect/token");

                // Enable the client credentials flow.
                options.AllowClientCredentialsFlow()
                    .AllowPasswordFlow()
                    .AllowAuthorizationCodeFlow();

                // Register the signing and encryption credentials.
                options.AddDevelopmentEncryptionCertificate()
                       .AddDevelopmentSigningCertificate();

                // Register the ASP.NET Core host and configure the ASP.NET Core options.
                options.UseAspNetCore()
                       .EnableTokenEndpointPassthrough()
                       .DisableTransportSecurityRequirement(); //关闭强制https验证

            })

            // Register the OpenIddict validation components.
            .AddValidation(options =>
            {
                // Import the configuration from the local OpenIddict server instance.
                options.UseLocalServer();

                // Register the ASP.NET Core host.
                options.UseAspNetCore();
            });

        builder.Services.AddHostedService<OpenIddictWorker>();

        return builder;
    }
}
