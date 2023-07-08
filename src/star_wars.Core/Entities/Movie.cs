namespace star_wars.Core.Entities;

public class Movie
{
    public int Id { get; set; }
    public string Title { get; set; }
    public ICollection<Character> Characters { get; set; }
}