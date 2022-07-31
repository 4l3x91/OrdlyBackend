namespace OrdlyBackend.Models
{
    public class Rank : BaseEntity
    {
        public int Id { get; set; }
        public string RankName { get; set; }
        public int MaxRating { get; set; }

        public Rank()
        {

        }
    }
}