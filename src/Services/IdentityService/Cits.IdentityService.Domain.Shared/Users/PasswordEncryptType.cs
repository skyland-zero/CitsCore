namespace Cits.IdentityService.Domain.Shared.Users;

public enum PasswordEncryptType
{
    /// <summary>
    /// 32位MD5加密
    /// </summary>
    MD5Encrypt32 = 2,

    /// <summary>
    /// 标准标识密码哈希
    /// 参考代码：https://andrewlock.net/exploring-the-asp-net-core-identity-passwordhasher/
    /// </summary>
    PasswordHasher = 3,
}
