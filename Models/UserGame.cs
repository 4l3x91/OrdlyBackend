using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace OrdlyBackend.Models
{
    public class UserGame
    {
        public int UserGameId { get; set; }
        public int UserId { get; set; }
        public int DailyWordId { get; set; }
        public bool isCompleted { get; set; }

        public UserGame()
        {

        }

        public static implicit operator UserGame(EntityEntry<UserGame> v)
        {
            throw new NotImplementedException();
        }
    }
}
