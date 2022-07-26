namespace OrdlyBackend.Models
{
    public class UserGame
    {
        public int UserGameId { get; set; }
        public int UserId { get; set; }
        public int DailyWordId { get; set; }
        public bool isCompleted { get; set; }
    }
}
