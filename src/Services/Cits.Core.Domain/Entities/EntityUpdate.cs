namespace Cits.Core.Domain.Entities;

public class EntityUpdate : EntityAdd, IEntityUpdate
{
    /// <summary>
    /// 修改用户Id
    /// </summary>
    public virtual string? ModifiedUserId { get; set; }

    /// <summary>
    /// 修改用户账号
    /// </summary>
    public virtual string? ModifiedUserName { get; set; }

    /// <summary>
    /// 修改时间
    /// </summary>
    public virtual DateTime? ModifiedTime { get; set; }
}
