namespace OrdlyBackend.Models;

public abstract class BaseEntity
{
    public virtual DateTime RowCreated { get; set; }
    public virtual DateTime RowModified { get; set; }
    public virtual int RowVersion { get; set; }
}
