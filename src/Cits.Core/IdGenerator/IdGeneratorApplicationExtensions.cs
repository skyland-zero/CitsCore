using Cits.Core.Configuration;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Cits.Core.IdGenerator;

public static class IdGeneratorApplicationExtensions
{
    public static IHostApplicationBuilder AddIdGenerator(this IHostApplicationBuilder builder)
    {
        builder.Services.AddOptions();
        builder.Services.Configure<ServiceOptions>(builder.Configuration.GetSection(ServiceOptions.ConfigurationSection));
        if (string.IsNullOrWhiteSpace(builder.Configuration.GetConnectionString("Redis")))
        {
            builder.Services.AddSingleton<IIdGeneratorWorker, DefaultIdGeneratorWorker>();
        }
        else
        {
            builder.Services.AddSingleton<IIdGeneratorWorker, RedisIdGeneratorWorker>();
        }

        builder.Services.AddHostedService<WorkerNodeHostedService>();

        return builder;
    }
}