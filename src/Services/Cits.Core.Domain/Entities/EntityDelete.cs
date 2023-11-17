namespace Cits.Core.Domain.Entities;

public class EntityDelete : IDelete
{
    /// <summary>
    /// 是否删除
    /// </summary>
    public virtual bool IsDeleted { get; set; } = false;
}
