namespace OrdlyBackend.DTOs.v1
{
    public class GuessResponse : DailyGameDTO
    {
        public int[] Result { get; set; }
        public bool isCompleted { get; set; }
    }
}
