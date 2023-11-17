using Cits.Core.Domain.Entities;
using FreeSql.Extensions.EfCoreFluentApi;

namespace Cits.IdentityService.Domain;

public static class EntityBaseConfigurationExtension
{
    public static void ConfigureBase<T>(this EfCoreTableFluent<T> eb) where T : EntityBase
    {
        eb.HasKey(x => x.Id);
        eb.Property(x => x.Id).HasMaxLength(40);

        eb.Property(x => x.CreatedUserId).HasMaxLength(128);
        eb.Property(x => x.CreatedUserName).HasMaxLength(128);

        eb.Property(x => x.ModifiedUserId).HasMaxLength(128);
        eb.Property(x => x.ModifiedUserName).HasMaxLength(128);
    }


}
