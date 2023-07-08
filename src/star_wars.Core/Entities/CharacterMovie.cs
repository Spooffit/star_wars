namespace star_wars.Core.Entities;

public class CharacterMovie
{
    public ICollection<Character> Character { get; set; }
    public ICollection<Movie> Movie { get; set; }
}