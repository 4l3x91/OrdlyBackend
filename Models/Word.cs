namespace OrdlyBackend.Models;

public class Word
{
    public int WordId { get; set; }
    public string Name { get; set; }
    public string Category { get; set; }
    public Word() { }
    public Word(int wordId, string name)
    {
        WordId = wordId;
        Name = name;
    }
}