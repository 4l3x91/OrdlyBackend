namespace OrdlyBackend.DTOs.v1.UserGameDTOs
{
    public class UserHistoryDTO
    {
        public UserGameDTO CurrentGame { get; set; }
        public List<UserGameDTO> PreviousGames { get; set; } = new List<UserGameDTO>();
    }
}
