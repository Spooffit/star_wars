using AutoMapper;
using star_wars.Application.Common.Interfaces.Repositories;
using star_wars.Application.Common.Interfaces.Services;
using star_wars.Application.Common.Models.ViewModels.Movie;

namespace star_wars.Infrastructure.Services;

public class MovieService : IMovieService
{
    private readonly IMovieRepository _movieRepository;
    private readonly IMapper _mapper;

    public MovieService(
        IMovieRepository movieRepository,
        IMapper mapper)
    {
        _movieRepository = movieRepository;
        _mapper = mapper;
    }
    
    public async Task<List<MovieViewModel>> GetAllMoviesAsync()
    {
        var movies = await _movieRepository.GetAllMoviesAsync();
        return _mapper.Map<List<MovieViewModel>>(movies);
    }
}