using star_wars.Core.Entities;

namespace star_wars.Application.Common.Interfaces.Repositories;

public interface IMovieRepository
{
    Task<List<Movie>> GetAllMoviesAsync();
}