using Cits.IdentityService.Domain.Users;

namespace Cits.IdentityService.Domain.FreeSql;

public static class AppInitExtension
{
    /// <summary>
    /// 初始化方法
    /// </summary>
    /// <param name="freeSql"></param>
    public static void Init(this IFreeSql freeSql)
    {
        ApplyConfigurations(freeSql);
        SyncStructure(freeSql);
        InitSeedData(freeSql);
    }

    /// <summary>
    /// 批量配置实体配置类(根据程序集扫描)
    /// </summary>
    /// <param name="freeSql"></param>
    private static void ApplyConfigurations(IFreeSql freeSql)
    {
        freeSql.CodeFirst.ApplyConfigurationsFromAssembly(typeof(EntityBaseConfigurationExtension).Assembly);
    }

    /// <summary>
    /// 同步表
    /// </summary>
    /// <param name="freeSql"></param>
    private static void SyncStructure(IFreeSql freeSql)
    {
        //TODO 应该单独启动一个控制台之类的进行迁移，单独启动进行表结构迁移
        freeSql.CodeFirst.SyncStructure<User>();
    }

    /// <summary>
    /// 初始化种子数据
    /// </summary>
    /// <param name="freeSql"></param>
    private static void InitSeedData(IFreeSql freeSql)
    {

    }
}
