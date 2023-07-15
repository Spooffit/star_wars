using star_wars.Application.Common.Models.ViewModels.Movie;

namespace star_wars.Application.Common.Models.ViewModels.Character;

public class InfoCharacterViewModel
{
    public string Name { get; set; }
    public string OriginName { get; set; }
    public int Birthdate { get; set; }
    public string Planet { get; set; }
    public string Gender { get; set; }
    public string Species { get; set; }
    public float Height { get; set; }
    public string HairColor { get; set; }
    public string EyeColor { get; set; }
    public string Description { get; set; }
    public List<MovieViewModel> Movies { get; set; }
}