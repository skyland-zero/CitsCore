using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Logging;

namespace Microsoft.Extensions.Hosting;

public static class ApplicationLifetimeExtension
{
    public static void RegisterApplicationLifeTimeEvents(this WebApplication app)
    {
        app.Lifetime.ApplicationStarted.Register(() => OnStarted(app.Logger));
        app.Lifetime.ApplicationStopping.Register(() => OnStopping(app.Logger));
        app.Lifetime.ApplicationStopped.Register(() => OnStopped(app.Logger));
    }

    public static void OnStarted(ILogger logger)
    {
        logger.StartedApp(DateTime.Now);
    }

    public static void OnStopping(ILogger logger)
    {
        logger.StoppingApp(DateTime.Now);
    }

    public static void OnStopped(ILogger logger)
    {
        logger.StoppedApp(DateTime.Now);
    }
}

public static partial class ApplicationLifetimeLogs
{
    [LoggerMessage(LogLevel.Information, "{Time} 程序正在启动...")]
    public static partial void StartingApp(this ILogger logger, DateTime time);


    [LoggerMessage(LogLevel.Information, "{Time} 程序已启动完成")]
    public static partial void StartedApp(this ILogger logger, DateTime time);


    [LoggerMessage(LogLevel.Information, "{Time} 程序正在停止...")]
    public static partial void StoppingApp(this ILogger logger, DateTime time);

    [LoggerMessage(LogLevel.Information, "{Time} 程序已停止")]
    public static partial void StoppedApp(this ILogger logger, DateTime time);


}
