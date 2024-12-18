using System.Reflection.Metadata.Ecma335;
using AutoMapper;
using FilmRentalStore.DTO;
using FilmRentalStore.Models;
using Microsoft.EntityFrameworkCore;

namespace FilmRentalStore.Services
{
    public class FilmService : IFilmRepository
    {

        private readonly Sakila12Context _context;
        private readonly IMapper _mapper;
        public FilmService(Sakila12Context context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;

        }

       
        #region AddFilm
        public async Task AddFilm(FilmDTO film)
        {
            var filmData = _mapper.Map<Film>(film);
            _context.Films.Add(filmData);
            await _context.SaveChangesAsync();
        }
        #endregion

        #region SearchFilmsByTitle
        public async Task<List<FilmDTO>> SearchFilmsByTitle(string title)
        {
            var films = await _context.Films.Where(f => f.Title==title).ToListAsync();

            return _mapper.Map<List<FilmDTO>>(films);
        }
        #endregion

        #region SearchFilmsByReleaseYear
        public async Task<List<FilmDTO>> SearchFilmsByReleaseYear(string releaseYear)
        {
            var films = await _context.Films.Where(f => f.ReleaseYear == releaseYear).ToListAsync();

            return _mapper.Map<List<FilmDTO>>(films);
        }
        #endregion

        #region SearchFilmsByRentalDurationHigher
        public async Task<List<FilmDTO>> SearchFilmsByRentalDurationHigher(byte rentalduration)
        {
            var films = await _context.Films.Where(f => f.RentalDuration > rentalduration).ToListAsync();

            return _mapper.Map<List<FilmDTO>>(films);
        }
        #endregion

        #region SearchFilmsByRentalRateGreater
        public async Task<List<FilmDTO>> SearchFilmsByRentalRateGreater(decimal rate)
        {
            var films = await _context.Films.Where(f => f.RentalRate > rate).ToListAsync();

            return _mapper.Map<List<FilmDTO>>(films);
        }
        #endregion

        #region SearchFilmsByLengthGreater
        public async Task<List<FilmDTO>> SearchFilmsByLengthGreater(short length)
        {
            var films = await _context.Films.Where(f => f.Length > length).ToListAsync();

            return _mapper.Map<List<FilmDTO>>(films);
        }
        #endregion

        #region SearchFilmsByRentalDurationLower
        public async Task<List<FilmDTO>> SearchFilmsByRentalDurationLower(byte rentalduration)
        {
            var films = await _context.Films.Where(f => f.RentalDuration < rentalduration).ToListAsync();

            return _mapper.Map<List<FilmDTO>>(films);
        }
        #endregion

        #region SearchFilmsByRentalRateLower
        public async Task<List<FilmDTO>> SearchFilmsByRentalRateLower(decimal rate)
        {
            var films = await _context.Films.Where(f => f.RentalRate > rate).ToListAsync();

            return _mapper.Map<List<FilmDTO>>(films);
        }
        #endregion

        #region SearchFilmsByLengthLower
        public async Task<List<FilmDTO>> SearchFilmsByLengthLower(short length)
        {
            var films = await _context.Films.Where(f => f.Length < length).ToListAsync();

            return _mapper.Map<List<FilmDTO>>(films);
        }
        #endregion


        #region SearchFilmsByReleaseYearRange

        public async Task<List<FilmDTO>> SearchFilmsByReleaseYearRange(int from, int to)
        {
            int year = 0;
            var films = await _context.Films.Where(f => int.TryParse(f.ReleaseYear, out year) && year >= from && year <= to)
                .ToListAsync();

            return _mapper.Map<List<FilmDTO>>(films);
        }

        #endregion


        #region SearchFilmsByRatingLower
        public async Task<List<FilmDTO>> SearchFilmsByRatingLower(decimal rating)
        {
            decimal filmRating = 0;
            var films = await _context.Films.Where(f => decimal.TryParse(f.Rating, out filmRating) && filmRating < rating)
                .ToListAsync();

            return _mapper.Map<List<FilmDTO>>(films);
        }
        #endregion

        #region SearchFilmsByRatingHigher
        public async Task<List<FilmDTO>> SearchFilmsByRatingHigher(decimal rating)
        {
            decimal filmRating = 0;
            var films = await _context.Films.Where(f => decimal.TryParse(f.Rating, out filmRating) && filmRating > rating)
                .ToListAsync();

            return _mapper.Map<List<FilmDTO>>(films);
        }
        #endregion


        #region SearchFilmsByLanguage
        public async Task<List<FilmDTO>> SearchFilmsByLanguage(string lang)
        {
            var films = await _context.Films.Join(_context.Languages,film => film.LanguageId,lang => lang.LanguageId,(film, lang) => new { film, lang })
            .Where(j => j.lang.Name == lang)
            .Select(j => j.film)
            .ToListAsync();

            return _mapper.Map<List<FilmDTO>>(films);
        }

        #endregion

        #region GetFilmCountByReleaseYear
        public async Task<Dictionary<string, int>> GetFilmCountByReleaseYear()
        {
           
            var films = await _context.Films.ToListAsync();
           
            var filmsByYear = films .GroupBy(f => f.ReleaseYear) .ToDictionary(n => n.Key, n => n.Count());

            //return _mapper.Map<Dictionary<string,int>>(films);
            return filmsByYear;
        }
        #endregion

        #region GetActorsByFilmId
        public async Task<List<ActorDTO>> GetActorsByFilmId(int filmId)
        {
            var actors = await _context.Films.Join(_context.FilmActors, film => film.FilmId, filmActor => filmActor.FilmId, (film, filmActor) => new { film, filmActor })
                                              .Join(_context.Actors, j => j.filmActor.ActorId, actor => actor.ActorId, (j, actor) => new { j.film, actor })
                                              .Where(j => j.film.FilmId == filmId)
                                              .Select(j => new ActorDTO
                                              {
                                                  //ActorId = j.actor.ActorId,
                                                  //FirstName = j.actor.FirstName,
                                                  //LastName=j.actor.LastName
                                              })
                                              .ToListAsync();

            return _mapper.Map<List<ActorDTO>>(actors);
        }
        #endregion


        #region GetFilmsByCategory
        public async Task<List<FilmDTO>> GetFilmsByCategory(string category)
        {
            var films = await _context.Films.Join(_context.FilmCategories, film => film.FilmId, filmCategory => filmCategory.FilmId, (film, filmCategory) => new { film, filmCategory })
                                             .Join(_context.Categories, j => j.filmCategory.CategoryId, category => category.CategoryId, (j, category) => new { j.film, category })
                                             .Where(j => j.category.Name == category)
                                             .Select(j => j.film)
                                             .ToListAsync();

            return _mapper.Map<List<FilmDTO>>(films);
        }
        #endregion












    }
}
