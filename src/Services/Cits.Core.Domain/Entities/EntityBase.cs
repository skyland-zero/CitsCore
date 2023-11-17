namespace Cits.Core.Domain.Entities;

public class EntityBase : EntityUpdate, IEntityAdd, IEntityUpdate, IDelete
{
    /// <summary>
    /// 主键
    /// </summary>
    public virtual string Id { get; set; }

    /// <summary>
    /// 是否删除
    /// </summary>
    public virtual bool IsDeleted { get; set; } = false;
}
