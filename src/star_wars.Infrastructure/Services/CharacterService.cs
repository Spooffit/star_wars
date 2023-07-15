using AutoMapper;
using star_wars.Application.Common.Interfaces.Repositories;
using star_wars.Application.Common.Interfaces.Services;
using star_wars.Application.Common.Models.ViewModels.Character;
using star_wars.Application.Common.Models.ViewModels.Movie;
using star_wars.Core.Entities;

namespace star_wars.Infrastructure.Services;

public class CharacterService : ICharacterService
{
    private readonly ICharacterRepository _characterRepository;
    private readonly IMovieRepository _movieRepository;
    private readonly IMapper _mapper;

    public CharacterService(
        ICharacterRepository characterRepository,
        IMovieRepository movieRepository,
        IMapper mapper)
    {
        _characterRepository = characterRepository;
        _movieRepository = movieRepository;
        _mapper = mapper;
    }

    public async Task<IndexCharacterListViewModel> GetAllIndexCharactersAsync()
    {
        var entities = await _characterRepository.GetAllCharactersAsync();
        var entitiesViewModel = _mapper.Map<List<IndexCharacterViewModel>>(entities);

        var viewModel = new IndexCharacterListViewModel
        {
            Characters = entitiesViewModel
        };

        return viewModel;
    }

    async Task<InfoCharacterViewModel> ICharacterService.GetInfoCharacterByIdAsync(int id)
    {
        var entity = await _characterRepository.GetCharacterByIdAsync(id);
        var viewModel = _mapper.Map<InfoCharacterViewModel>(entity);

        return viewModel;
    }

    public async Task<EditCharacterViewModel> GetEditCharacterByIdAsync(int id)
    {
        var entity = await _characterRepository.GetCharacterByIdAsync(id);
        var allMovies = await _movieRepository.GetAllMoviesAsync();
        
        var viewModel = _mapper.Map<EditCharacterViewModel>(entity);
        
        var characterOwnsMovieListTitle = entity.Movies
            .Select(x => x.Title);
        
        var moviesViewModels = _mapper.Map<List<MovieViewModel>>(allMovies.Where(m => !characterOwnsMovieListTitle.Contains(m.Title)).ToList());
        
        viewModel.OwnsMovies = string.Join(",",characterOwnsMovieListTitle);
        viewModel.RestAllMovies = moviesViewModels;

        return viewModel;
    }

    public async Task<AddCharacterViewModel> GetAddCharacterAsync()
    {
        var allMovies = await _movieRepository.GetAllMoviesAsync();
        var allViewModelMovies = _mapper.Map<List<MovieViewModel>>(allMovies);
        
        var viewModel = new AddCharacterViewModel
        {
            RestAllMovies = allViewModelMovies
        };

        return viewModel;
    }

    public async Task<AddCharacterViewModel> GetInfoCharacterByIdAsync(int id)
    {
        var entity = await _characterRepository.GetCharacterByIdAsync(id);
        var viewModel = _mapper.Map<AddCharacterViewModel>(entity);

        return viewModel;
    }

    public async Task<AddCharacterViewModel> AddCharacterAsync(AddCharacterViewModel viewModel)
    {
        var entity = _mapper.Map<Character>(viewModel);
        if (viewModel.OwnsMovies is not null)
        {
            var selectedMoviesTitles = viewModel.OwnsMovies.Split(",");
            var allExistingMovies = await _movieRepository.GetAllMoviesAsync();
            var selectedMovies = allExistingMovies.Where(m => selectedMoviesTitles.Contains(m.Title)).ToList();
            
            entity.Movies = selectedMovies;
        }
        else
            entity.Movies.Clear();
        
        await _characterRepository.AddCharacterAsync(entity);
        return viewModel;
    }

    public async Task<EditCharacterViewModel> UpdateCharacterAsync(EditCharacterViewModel viewModel)
    {
        var existingEntity = await _characterRepository.GetCharacterByIdAsync(viewModel.Id);
        
        _mapper.Map(viewModel, existingEntity);

        if (viewModel.OwnsMovies is not null)
        {
            var selectedMoviesTitles = viewModel.OwnsMovies.Split(",");
            var allExistingMovies = await _movieRepository.GetAllMoviesAsync();
            var selectedMovies = allExistingMovies.Where(m => selectedMoviesTitles.Contains(m.Title)).ToList();
            
            existingEntity.Movies = selectedMovies;
        }
        else
            existingEntity.Movies.Clear();

        await _characterRepository.UpdateCharacterAsync(existingEntity);
        return viewModel;
    }



    public async Task<bool> DeleteCharacterByIdAsync(int id)
    {
        return await _characterRepository.DeleteCharacterByIdAsync(id);
    }
}


