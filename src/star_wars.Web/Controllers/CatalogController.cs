using Microsoft.AspNetCore.Mvc;
using star_wars.Application.Common.Interfaces.Services;
using star_wars.Application.Common.Models.ViewModels.Character;
using X.PagedList;


namespace star_wars.Web.Controllers;

public class CatalogController : Controller
{
    private readonly ICharacterService _characterService;

    public CatalogController(
        ICharacterService characterService)
    {
        _characterService = characterService;
    }
    
    [HttpGet]
    public async Task<ActionResult> Index(int page = 1, int pageSize = 8)
    {
        var viewModel = await _characterService.GetPagedIndexCharactersAsync(page, pageSize);
        var pagedList = new StaticPagedList<IndexCharacterViewModel>(viewModel, page, pageSize, viewModel.TotalItemCount);

        return View(pagedList);
    }
    
    [HttpGet]
    public async Task<IActionResult> Info(int id)
    {
        var viewModel = await _characterService.GetInfoCharacterByIdAsync(id);

        return View(viewModel);
    }
    
    [HttpGet]
    public async Task<IActionResult> Edit(int id)
    {
        var viewModel = await _characterService.GetEditCharacterByIdAsync(id);

        return View(viewModel);
    }
    
    [HttpPost]
    public async Task<IActionResult> Edit(EditCharacterViewModel viewModel)
    {
        await _characterService.UpdateCharacterAsync(viewModel);
        
        return RedirectToAction(nameof(Index));
    }
    
    
    [HttpGet]
    public async Task<IActionResult> Add()
    {
        var viewModel = await _characterService.GetAddCharacterAsync();
        return View(viewModel);
    }
    
    [HttpPost]
    public async Task<IActionResult> Add(AddCharacterViewModel viewModel)
    {
        await _characterService.AddCharacterAsync(viewModel);

        return RedirectToAction(nameof(Index));
    }
    
    
    [HttpPost]
    public async Task<IActionResult> Delete(int id)
    {
        await _characterService.DeleteCharacterByIdAsync(id);
        return RedirectToAction(nameof(Index));
    }
}