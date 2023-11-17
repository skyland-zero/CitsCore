namespace Cits.IdentityService.Domain.Shared.Users;

/// <summary>
/// 用户状态
/// </summary>
public enum UserStatus
{
    /// <summary>
    /// 正常状态
    /// </summary>
    Default = 1,

    /// <summary>
    /// 待修改密码
    /// </summary>
    WaitChangePassword,

    /// <summary>
    /// 待激活
    /// </summary>
    WaitActive
}
