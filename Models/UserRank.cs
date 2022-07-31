namespace OrdlyBackend.Models
{
    public class UserRank : BaseEntity
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int Rating { get; set; }
    }
}
