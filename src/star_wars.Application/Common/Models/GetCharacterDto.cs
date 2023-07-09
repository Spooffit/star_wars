using star_wars.Core.Entities;

namespace star_wars.Application.Common.Models;

public class GetCharacterDto
{
    public int Id { get; }
    public string Name { get; }
    public string OriginName { get; }
    public int Birthdate { get; }
    public Planet Planet { get; }
    public string Gender { get; }
    public string Species { get; }
    public float Height { get; }
    public string HairColor { get; }
    public string EyeColor { get; }
    public string Description { get; }
    public ICollection<Movie> Movies { get; }
}