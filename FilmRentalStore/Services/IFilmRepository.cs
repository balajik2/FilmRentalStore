using FilmRentalStore.DTO;
using FilmRentalStore.Models;
using Microsoft.VisualBasic;

namespace FilmRentalStore.Services
{
    public interface IFilmRepository
    {
        Task AddFilm(FilmDTO film);

        Task<List<FilmDTO>> SearchFilmsByTitle(string title);

        Task<List<FilmDTO>> SearchFilmsByReleaseYear(string releaseYear);

        Task<List<FilmDTO>> SearchFilmsByRentalDurationGreater(byte rentalDuration);

        Task<List<FilmDTO>> SearchFilmsByRentalRateGreater(decimal rentalRate);

        Task<List<FilmDTO>> SearchFilmsByLengthGreater(short length);

        Task<List<FilmDTO>> SearchFilmsByRentalDurationLower(byte rentalduration);

        Task<List<FilmDTO>> SearchFilmsByRentalRateLower(decimal rate);

        Task<List<FilmDTO>> SearchFilmsByLengthLower(short length);

        Task<List<FilmDTO>> SearchFilmsByReleaseYearRange(int from, int to);

        Task<List<FilmDTO>> SearchFilmsByRatingLower(string rating);

        Task<List<FilmDTO>> SearchFilmsByRatingHigher(string rating);

        Task<List<FilmDTO>> SearchFilmsByLanguage(string language);

        Task<Dictionary<string, int>> GetFilmCountByReleaseYear();

        Task<List<ActorDTO>> GetActorsByFilmId(int filmId);
        Task<List<FilmDTO>> GetFilmsByCategory(string category);

        Task UpdateTitleOfFilm(int filmid,string title);

        Task UpdateReleaseYearOfFilm(int filmid, string releaseyear);

        Task UpdateRentalDurationOfFilm(int filmid, byte rentalduration);

        Task UpdateRentalRateOfFilm(int filmid, decimal rentalrate);

        Task UpdateRatingOfFilm(int filmid, string rating);

        Task UpdateLanguageOfFilm(int filmId, LanguageDTO language);

        Task UpdateCategoryOfFilm(int filmId, CategoryDTO category);

        }
}
