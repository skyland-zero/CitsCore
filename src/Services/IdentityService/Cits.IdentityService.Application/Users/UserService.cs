using Cits.Core.Dto;
using Cits.IdentityService.Application.Contracts.User;
using Cits.IdentityService.Domain.Shared.Users;
using Cits.IdentityService.Domain.Users;
using Microsoft.AspNetCore.Identity;

namespace Cits.IdentityService.Application.Users;

public class UserService : IUserService
{
    private readonly IFreeSql _freeSql;
    private readonly IPasswordHasher<User> _passwordHasher;

    public UserService(IFreeSql freeSql, IPasswordHasher<User> passwordHasher)
    {
        _freeSql = freeSql;
        _passwordHasher = passwordHasher;
    }

    public async Task<IResultOutput> CreateUserAsync(User user, CancellationToken cancellationToken)
    {
        user.Id = Guid.NewGuid().ToString();
        HashPassword(user);
        await _freeSql.Insert(user).ExecuteAffrowsAsync(cancellationToken);
        return ResultOutput.Ok();
    }

    private void HashPassword(User user)
    {
        switch (user.PasswordEncryptType)
        {
            case PasswordEncryptType.PasswordHasher:
                user.Password = _passwordHasher.HashPassword(user, user.Password);
                break;
        }
    }
}
