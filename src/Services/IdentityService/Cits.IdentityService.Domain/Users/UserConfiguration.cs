using FreeSql.Extensions.EfCoreFluentApi;

namespace Cits.IdentityService.Domain.Users;

public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EfCoreTableFluent<User> eb)
    {
        eb.ConfigureBase();
        eb.Property(x => x.UserName)
            .IsRequired()
            .HasMaxLength(128);
        eb.Property(x => x.Password)
            .IsRequired()
            .HasMaxLength(4000);
        eb.Property(x => x.Email)
            .HasMaxLength(200);
        eb.Property(x => x.PhoneNumber)
            .HasMaxLength(15);
    }
}
