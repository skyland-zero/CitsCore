using Cits.Core.Domain.Entities;
using Cits.IdentityService.Domain.Shared.Users;

namespace Cits.IdentityService.Domain.Users;

public class User : EntityBase
{
    /// <summary>
    /// 账号
    /// </summary>
    public string UserName { get; set; }

    /// <summary>
    /// 密码
    /// </summary>
    public string Password { get; set; }

    /// <summary>
    /// 密码加密类型(默认md5加密)
    /// </summary>
    public PasswordEncryptType PasswordEncryptType { get; set; } = PasswordEncryptType.PasswordHasher;

    /// <summary>
    /// 邮箱
    /// </summary>
    public string? Email { get; set; }

    /// <summary>
    /// 手机号码
    /// </summary>
    public string? PhoneNumber { get; set; }

    /// <summary>
    /// 头像
    /// </summary>
    public string? Avatar { get; set; }

    /// <summary>
    /// 用户状态
    /// </summary>
    public UserStatus Status { get; set; } = UserStatus.Default;

    /// <summary>
    /// 启用
    /// </summary>
    public bool IsEnable = true;
}
