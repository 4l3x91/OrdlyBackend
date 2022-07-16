namespace OrdlyBackend.Models;

public class User
{
    public int UserId { get; set; }
    public string TotalGames { get; set; }
    public string TotalWins { get; set; }
    public string CurrentStreak { get; set; }
    public string LongestStreak { get; set; }
    public User() { }
    public User(int userId, string totalGames, string totalWins, string currentStreak, string longestStreak)
    {
        UserId = userId;
        TotalGames = totalGames;
        TotalWins = totalWins;
        CurrentStreak = currentStreak;
        LongestStreak = longestStreak;
    }
}