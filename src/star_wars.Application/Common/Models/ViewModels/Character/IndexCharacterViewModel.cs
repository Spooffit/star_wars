namespace star_wars.Application.Common.Models.ViewModels.Character;

public class IndexCharacterViewModel
{
    public Guid OwnerId { get; set; }
    public int Id { get; set; }
    public string Name { get; set; }
    public string OriginName { get; set; }
}