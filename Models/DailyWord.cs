namespace OrdlyBackend.Models
{
    public class DailyWord : BaseEntity
    {
        public int Id { get; set; }
        public int WordId { get; set; }
        public DateTime Date { get; set; }

        public DailyWord()
        {

        }

    }
}
