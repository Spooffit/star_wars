using star_wars.Application.Common.Models.ViewModels.Movie;

namespace star_wars.Application.Common.Interfaces.Services;

public interface IMovieService
{
    Task<List<MovieViewModel>> GetAllMoviesAsync();
}