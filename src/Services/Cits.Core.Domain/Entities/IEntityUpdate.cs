namespace Cits.Core.Domain.Entities;

public interface IEntityUpdate
{
    /// <summary>
    /// 修改用户Id
    /// </summary>
    string? ModifiedUserId { get; set; }

    /// <summary>
    /// 修改用户账号
    /// </summary>
    string? ModifiedUserName { get; set; }

    /// <summary>
    /// 修改时间
    /// </summary>
    DateTime? ModifiedTime { get; set; }
}
