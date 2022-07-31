namespace OrdlyBackend.Models
{
    public class Rank : BaseEntity
    {
        public int RankId { get; set; }
        public string RankName { get; set; }
        public int MaxRating { get; set; }

        public Rank()
        {

        }
    }
}