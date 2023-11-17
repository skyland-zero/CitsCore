namespace Cits.Core.Domain.Entities;

public interface IEntityAdd
{
    /// <summary>
    /// 创建用户Id
    /// </summary>
    string? CreatedUserId { get; set; }

    /// <summary>
    /// 创建用户账号
    /// </summary>
    string? CreatedUserName { get; set; }

    /// <summary>
    /// 创建时间
    /// </summary>
    DateTime? CreatedTime { get; set; }
}
