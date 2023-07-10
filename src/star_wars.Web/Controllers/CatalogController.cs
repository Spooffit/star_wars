using Microsoft.AspNetCore.Mvc;
using star_wars.Application.Common.Interfaces.Services;


namespace star_wars.Web.Controllers;

public class CatalogController : Controller
{
    private readonly ICharacterService _characterService;
    private readonly ICharacterViewModelService _characterViewModelService;

    public CatalogController(
        ICharacterService service, 
        ICharacterViewModelService characterViewModelService)
    {
        _characterService = service;
        _characterViewModelService = characterViewModelService;
    }
    
    public async Task<IActionResult> Index()
    {
        var characters = await _characterService.GetAllCharactersAsync();
        var characterViewModels = _characterViewModelService.MapCharacterListToViewModel(characters);

        return View(characterViewModels);
    }
}