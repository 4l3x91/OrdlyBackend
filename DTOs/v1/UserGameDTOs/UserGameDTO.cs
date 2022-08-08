namespace OrdlyBackend.DTOs.v1.UserGameDTOs
{
    public class UserGameDTO
    {
        public int DailyGameId { get; set; }
        public List<GuessDTO> Guesses { get; set; } = new();
    }
}
