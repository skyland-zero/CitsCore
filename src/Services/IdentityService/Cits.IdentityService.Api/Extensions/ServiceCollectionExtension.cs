using Cits.IdentityService.Application.Auth;
using Cits.IdentityService.Application.Contracts.Auth;

namespace Microsoft.Extensions.Hosting;

public static class ServiceCollectionExtension
{
    public static IServiceCollection AddAppServices(this IServiceCollection services)
    {
        //扫描并注入所有程序服务
        services.Scan(scan => 
            scan.FromAssemblyOf<AuthService>()  //扫描实现类所在的程序集
            .AddClasses(classes => 
                classes.Where(t=>
                    t.Name.EndsWith("Service",StringComparison.OrdinalIgnoreCase) ||
                    t.Name.EndsWith("Services",StringComparison.OrdinalIgnoreCase)))
            .AsImplementedInterfaces()
            .WithScopedLifetime() //以Scope生命周期注入
        );
        
        return services;
    }
}