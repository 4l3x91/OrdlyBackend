namespace OrdlyBackend.Models;

public class User : BaseEntity
{
    public int Id { get; set; }
    public Guid UserKey { get; set; }

    public User() { }

}