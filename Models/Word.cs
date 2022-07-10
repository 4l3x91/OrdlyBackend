namespace WebApi.Models;

public class Word
{
    public int WordId { get; set; }
    public string Name { get; set; }
    public string Date { get; set; }
    public Word() { }
    public Word(int wordId, string name, string date)
    {
        WordId = wordId;
        Name = name;
        Date = date;

    }
}