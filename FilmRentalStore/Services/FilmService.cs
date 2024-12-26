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
            
                var films = await _context.Films.Where(f => f.Title == title).ToListAsync();

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

        #region SearchFilmsByRentalDurationGreater
        public async Task<List<FilmDTO>> SearchFilmsByRentalDurationGreater(byte rentalduration)
        {

           
                var films = await _context.Films.Where(f => f.RentalDuration > rentalduration).ToListAsync();

                return _mapper.Map<List<FilmDTO>>(films);
         
        }
        #endregion

        #region SearchFilmsByRentalRateGreater
        public async Task<List<FilmDTO>> SearchFilmsByRentalRateGreater(decimal rentalrate)
        {
            
                var films = await _context.Films.Where(f => f.RentalRate > rentalrate).ToListAsync();

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
            var films = await _context.Films.Where(f => f.RentalRate < rate).ToListAsync();

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
            
            var films = await _context.Films.Where(f => f.ReleaseYear != null && f.ReleaseYear.Length == 4)
                .ToListAsync(); 

            var filteredFilms = films.Where(f => int.TryParse(f.ReleaseYear, out var year) && year >= from && year <= to)
                .ToList();

            return _mapper.Map<List<FilmDTO>>(filteredFilms);
        }

        #endregion


        #region SearchFilmsByRatingLower
        public async Task<List<FilmDTO>> SearchFilmsByRatingLower(string rating)
        {
            decimal filmRating = 0;
            var films = await _context.Films.Where(f => f.Rating.CompareTo(rating)<0)
                .ToListAsync();

            return _mapper.Map<List<FilmDTO>>(films);
        }



        #endregion

        #region SearchFilmsByRatingHigher
        public async Task<List<FilmDTO>> SearchFilmsByRatingHigher(string rating)
        {
            decimal filmRating = 0;
            var films = await _context.Films.Where(f => f.Rating.CompareTo(rating) > 0)
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

            
            return filmsByYear;
        }
        #endregion

        #region GetActorsByFilmId

        public async Task<List<ActorDTO>> GetActorsByFilmId(int filmId)
        {
            var actors = await _context.Films
                .Join(
                    _context.FilmActors,
                    film => film.FilmId,
                    filmActor => filmActor.FilmId,
                    (film, filmActor) => new { film, filmActor }
                )
                .Join(
                    _context.Actors,
                    j => j.filmActor.ActorId,
                    actor => actor.ActorId,
                    (j, actor) => new { j.film, actor }
                )
                .Where(j => j.film.FilmId == filmId)
                .Select(j => new ActorDTO
                {
                    ActorId = j.actor.ActorId,
                    FirstName = j.actor.FirstName,
                    LastName = j.actor.LastName
                })
                .ToListAsync();

            return actors;
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

        #region AssignActorToFilm

        public async Task<List<ActorDTO>> AssignActorToFilm(int filmId, int actorId)
        {
            var film = await _context.Films.FindAsync(filmId);
            if (film == null)
            {
                throw new Exception("Film not found");
            }
            var actor = await _context.Actors.FindAsync(actorId);
            if (actor == null)
            {
                throw new Exception("Actor not found");
            }
            var filmActor = new FilmActor
            {
                FilmId = filmId,
                ActorId = actorId,
                LastUpdate = DateTime.UtcNow
            };

            _context.FilmActors.Add(filmActor);
            await _context.SaveChangesAsync();

            var updatedlist = await _context.Actors.Where(s => s.ActorId== actor.ActorId).Include(s=>s.FilmActors).ToListAsync();

            return _mapper.Map<List<ActorDTO>>(updatedlist);
        }


        #endregion

        #region UpdateTitleOfFilm

        public async Task<List<FilmDTO>> UpdateTitleOfFilm(int filmid,string title)
        {
            var res = await _context.Films.FindAsync(filmid);
            if (res != null)
            {

               res.Title=title;
            }
            await _context.SaveChangesAsync();

            var updatedlist = await _context.Films.Where(s => s.FilmId == res.FilmId).ToListAsync();

            return _mapper.Map<List<FilmDTO>>(updatedlist);
        }

        #endregion

        #region UpdateReleaseYearOfFilm

        public async Task<List<FilmDTO>> UpdateReleaseYearOfFilm(int filmid, string releaseyear)
        {
            var res = await _context.Films.FindAsync(filmid);
            if (res != null)
            {

                res.ReleaseYear = releaseyear;
            }
            await _context.SaveChangesAsync();

            var updatedlist = await _context.Films.Where(s => s.FilmId == res.FilmId).ToListAsync();

            return _mapper.Map<List<FilmDTO>>(updatedlist);
        }

        #endregion

        #region UpdateRentalDurationOfFilm

        public async Task<List<FilmDTO>> UpdateRentalDurationOfFilm(int filmid, byte rentalduration)
        {
            var res = await _context.Films.FindAsync(filmid);
            if (res != null)
            {

                res.RentalDuration = rentalduration;
            }
            await _context.SaveChangesAsync();
            var updatedlist = await _context.Films.Where(s => s.FilmId == res.FilmId).ToListAsync();

            return _mapper.Map<List<FilmDTO>>(updatedlist);
        }

        #endregion

        #region UpdateRentalRateOfFilm

        public async Task<List<FilmDTO>> UpdateRentalRateOfFilm(int filmid, decimal rentalrate)
        {
            var res = await _context.Films.FindAsync(filmid);
            if (res != null)
            {

                res.RentalRate=rentalrate;
            }
            await _context.SaveChangesAsync();

            var updatedlist = await _context.Films.Where(s => s.FilmId == res.FilmId).ToListAsync();

            return _mapper.Map<List<FilmDTO>>(updatedlist);
        }

        #endregion

        #region UpdateRatingOfFilm

        public async Task<List<FilmDTO>> UpdateRatingOfFilm(int filmid, string rating)
        {
            var res = await _context.Films.FindAsync(filmid);
            if (res != null)
            {

                res.Rating = rating;

            }
           await _context.SaveChangesAsync();
            var updatedlist = await _context.Films.Where(s => s.FilmId == res.FilmId).ToListAsync();

            return _mapper.Map<List<FilmDTO>>(updatedlist);


        }

        #endregion

        #region UpdateLanguageOfFilm

        public async Task UpdateLanguageOfFilm(int filmId,LanguageDTO language)
        {

            var languageToUpdate = await _context.Languages
                .FirstOrDefaultAsync(l => l.LanguageId == language.LanguageId); 

            
            if (languageToUpdate != null)
            {
               
                _context.Entry(languageToUpdate).CurrentValues.SetValues(language);

                
                await _context.SaveChangesAsync();
            }

           


        }

        #endregion

        #region UpdateCategoryOfFilm
        public async Task UpdateCategoryOfFilm(int filmId,int categoryid)
        {
           
            var filmCategory = await _context.FilmCategories
                .FirstOrDefaultAsync(fc => fc.FilmId == filmId);

            if (filmCategory == null)
            {
                throw new Exception("Film category not found.");
            }

           
            _context.FilmCategories.Remove(filmCategory);
            await _context.SaveChangesAsync(); 

         
            var newFilmCategory = new FilmCategory
            {
                FilmId = filmId,
                CategoryId = categoryid, 
                LastUpdate = DateTime.UtcNow
            };
            await _context.FilmCategories.AddAsync(newFilmCategory);
            await _context.SaveChangesAsync();
        }

        #endregion























    }
}
