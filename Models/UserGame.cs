namespace OrdlyBackend.Models
{
    public class UserGame
    {
        int Id { get; set; }
        int UserId { get; set; }
        int GameId { get; set; }
        bool isCompleted { get; set; }
    }
}
