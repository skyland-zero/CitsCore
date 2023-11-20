using Cits.IdentityService.Api.Workers;
using Cits.OpenIddict.FreeSql;
using FreeSql.Internal;

namespace Microsoft.Extensions.Hosting;

public static class OpenIddictExtension
{
    /// <summary>
    /// 配置（使用自定义的FreeSqlStore）
    /// </summary>
    /// <param name="builder"></param>
    /// <returns></returns>
    public static IHostApplicationBuilder ConfigureOpenIddictWithFreeSql(this IHostApplicationBuilder builder)
    {
        //配置并注入OpenIddict使用的FreeSql实例
        Func<IServiceProvider, IFreeSql<CitsOpenIddictFreeSqlMark>> fsqlFactory = r =>
        {
            IFreeSql<CitsOpenIddictFreeSqlMark> fsql = new FreeSql.FreeSqlBuilder()
                .UseConnectionString(FreeSql.DataType.Sqlite, @"Data Source=freedb_openid.db")
                .UseNameConvert(NameConvertType.PascalCaseToUnderscoreWithLower)
                //.UseMonitorCommand(cmd => Console.WriteLine($"Sql：{cmd.CommandText}"))//监听SQL语句
                .UseAutoSyncStructure(true) //自动同步实体结构到数据库，FreeSql不会扫描程序集，只有CRUD时才会生成表。
                .Build<CitsOpenIddictFreeSqlMark>();
            return fsql;
        };
        builder.Services.AddSingleton(fsqlFactory);
        //配置OpenIddit
        builder.Services.AddOpenIddict()

            // Register the OpenIddict core components.
            .AddCore(options =>
            {
                options.UseFreeSql();
            })

            // Register the OpenIddict server components.
            .AddServer(options =>
            {
                // Enable the token endpoint.
                options.SetTokenEndpointUris("connect/token");

                // Enable the client credentials flow.
                options.AllowClientCredentialsFlow()
                    .AllowPasswordFlow();
                //.AllowAuthorizationCodeFlow()
                //.AllowImplicitFlow()
                //.AllowRefreshTokenFlow()
                //.AllowDeviceCodeFlow()
                //.AllowHybridFlow();

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
