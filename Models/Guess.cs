namespace OrdlyBackend.Models;

public class Guess
{
    public int GuessId { get; set; }
    public int UserGameId { get; set; }
    public int WordId { get; set; }

    public Guess()
    {

    }
}
