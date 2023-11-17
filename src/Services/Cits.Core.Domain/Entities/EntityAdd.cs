namespace Cits.Core.Domain.Entities;

public class EntityAdd : IEntityAdd
{
    /// <summary>
    /// 创建用户Id
    /// </summary>
    public virtual string? CreatedUserId { get; set; }

    /// <summary>
    /// 创建用户账号
    /// </summary>
    public virtual string? CreatedUserName { get; set; }

    /// <summary>
    /// 创建时间
    /// </summary>
    public virtual DateTime? CreatedTime { get; set; }
}
