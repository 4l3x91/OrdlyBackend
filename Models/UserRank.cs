namespace OrdlyBackend.Models
{
    public class UserRank : BaseEntity
    {
        public int UserRankId { get; set; }
        public int UserId { get; set; }
        public int Rating { get; set; }
    }
}
