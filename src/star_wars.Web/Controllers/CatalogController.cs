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
    
    [HttpGet]
    public async Task<IActionResult> Index()
    {
        var characters = await _characterService.GetAllCharactersAsync();
        var characterViewModels = _characterViewModelService.MapCharacterListToViewModel(characters);

        return View(characterViewModels);
    }
    
    [HttpGet]
    public async Task<ActionResult> Info(int id)
    {
        var entity = await _characterService.GetCharacterByIdAsync(id);
        var characterViewModels = _characterViewModelService.MapCharacterToViewModel(entity);
        
        return View(characterViewModels);
    }
    
    [HttpPut]
    public async Task<IActionResult> Edit()
    {
        return View();
    }
    
    [HttpDelete]
    public async Task<IActionResult> Delete()
    {
        return View();
    }
}