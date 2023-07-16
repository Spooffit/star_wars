using star_wars.Application.Common.Models.ViewModels.Character;
using X.PagedList;

namespace star_wars.Application.Common.Models.ViewModels;

public class CatalogViewModel
{
    public StaticPagedList<IndexCharacterViewModel> Characters { get; set; }
    public List<string> MoviesTitles { get; set; }
}