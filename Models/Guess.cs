namespace OrdlyBackend.Models;

public class Guess : BaseEntity
{
    public int Id { get; set; }
    public int UserGameId { get; set; }
    public int WordId { get; set; }

    public Guess()
    {

    }
}
