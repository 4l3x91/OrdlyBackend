namespace OrdlyBackend.Models;

public class User : BaseEntity
{
    public int UserId { get; set; }
    public Guid UserKey { get; set; }

    public User() { }

}