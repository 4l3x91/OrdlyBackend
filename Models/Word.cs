namespace WebApi.Models;

public class Word
{
    public string Id { get; set; }
    public string Name { get; set; }
    public Word() { }
    public Word(string id, string name)
    {
        Id = id;
        Name = Name;
    }
}