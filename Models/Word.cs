namespace OrdlyBackend.Models;

public class Word : BaseEntity
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Category { get; set; }
    public Word() { }
    public Word(int wordId, string name)
    {
        Id = wordId;
        Name = name;
    }
}