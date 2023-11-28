using Cits.Core.Dto;
using Cits.IdentityService.Application.Contracts.User.Dto;

namespace Cits.IdentityService.Application.Contracts.User;

public interface IUserService
{
    Task<IResultOutput> CreateUserAsync(Domain.Users.User user, CancellationToken cancellationToken);
}