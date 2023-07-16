using Microsoft.AspNetCore.Mvc;
using star_wars.Application.Common.Interfaces.Services;
using star_wars.Application.Common.Models.ViewModels;
using star_wars.Application.Common.Models.ViewModels.Character;
using X.PagedList;


namespace star_wars.Web.Controllers;

public class CatalogController : Controller
{
    private readonly ICharacterService _characterService;
    private readonly IMovieService _movieService;

    public CatalogController(
        ICharacterService characterService, 
        IMovieService movieService)
    {
        _characterService = characterService;
        _movieService = movieService;
    }
    
    [HttpGet]
    public async Task<ActionResult> Index(
        int? searchBirthDateFrom,
        int? searchBirthDateTo,
        string? searchPlanet,
        string? searchMovies,
        string? searchGender,
        int page = 1,
        int pageSize = 9)
    {
        var pagedCharacterViewModel = await _characterService.GetPagedIndexCharactersAsync(
            searchBirthDateFrom,
            searchBirthDateTo,
            searchPlanet,
            searchMovies,
            searchGender,
            page,
            pageSize);

        var pagedList = new StaticPagedList<IndexCharacterViewModel>(
            pagedCharacterViewModel, 
            page, 
            pageSize, 
            pagedCharacterViewModel.TotalItemCount);

        var moviesViewModel = await _movieService.GetAllMoviesAsync();
        
        var viewModel = new CatalogViewModel
        {
            Characters = pagedList,
            MoviesTitles = moviesViewModel.Select(m => m.Title).ToList()
        };

        return View(viewModel);
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