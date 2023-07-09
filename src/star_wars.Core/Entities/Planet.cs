namespace star_wars.Core.Entities;

public class Planet
{
    public int Id { get; set; }
    public string Name { get; set; }
    public ICollection<Character> Characters { get; set; }
}