namespace WebApi.Models;

public class User
{
    public string Id { get; set; }
    public string TotalGames { get; set; }
    public string TotalWins { get; set; }
    public string CurrentStreak { get; set; }
    public string LongestStreak { get; set; }
    public User() { }
    public User(string id, string totalGames, string totalWins, string currentStreak, string longestStreak)
    {
        Id = id;
        TotalGames = totalGames;
        TotalWins = totalWins;
        CurrentStreak = currentStreak;
        LongestStreak = longestStreak;
    }
}