using Microsoft.EntityFrameworkCore;

namespace Cits.IdentityService.Domain.EntityFramework;

public class OpenIddictDbContext : DbContext
{
    public OpenIddictDbContext(DbContextOptions<OpenIddictDbContext> options)
            : base(options)
    {
    }
}
