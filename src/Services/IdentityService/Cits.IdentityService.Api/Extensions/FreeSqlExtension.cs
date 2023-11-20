using Cits.IdentityService.Domain.FreeSql;

namespace Microsoft.Extensions.Hosting;
public static class FreeSqlExtension
{
    /// <summary>
    /// 配置、注入FreeSql服务
    /// </summary>
    /// <param name="builder"></param>
    /// <returns></returns>
    public static IHostApplicationBuilder ConfigureFreeSql(this IHostApplicationBuilder builder)
    {
        Func<IServiceProvider, IFreeSql> fsqlFactory = r =>
        {
            IFreeSql fsql = new FreeSql.FreeSqlBuilder()
                .UseConnectionString(FreeSql.DataType.Sqlite, @"Data Source=freedb.db")
                .UseMonitorCommand(cmd => Console.WriteLine($"Sql：{cmd.CommandText}"))//监听SQL语句
                .UseAutoSyncStructure(true) //自动同步实体结构到数据库，FreeSql不会扫描程序集，只有CRUD时才会生成表。
                .Build();
            fsql.CreateUnitOfWork();
            return fsql;
        };
        builder.Services.AddSingleton<IFreeSql>(fsqlFactory);
        return builder;
    }

    public static WebApplication InitFreeSql(this WebApplication app)
    {
        //在项目启动时，从容器中获取IFreeSql实例，并执行一些操作：同步表，种子数据,FluentAPI等
        using (IServiceScope serviceScope = app.Services.CreateScope())
        {
            var fsql = serviceScope.ServiceProvider.GetRequiredService<IFreeSql>();
            fsql.Init();
        }
        return app;
    }

}
