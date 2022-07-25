namespace OrdlyBackend.DTOs.v2
{
    public class GuessResponse2 : DailyGame2
    {
        public int[] Result { get; set; }
        public bool isCompleted { get; set; }
    }
}
