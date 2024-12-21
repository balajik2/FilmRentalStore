using System.ComponentModel.DataAnnotations;
using FilmRentalStore.DTO;
using FilmRentalStore.Models;
using FilmRentalStore.Services;
using FluentValidation;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Migrations.Operations;

namespace FilmRentalStore.Controllers
{
    [Route("api/films")]
    [ApiController]
    public class FilmController : ControllerBase
    {
        private readonly IFilmRepository _filmRepository;
        private readonly IValidator<FilmDTO> _validator;

        public FilmController(IFilmRepository filmRepository,IValidator<FilmDTO> validator)
        {
            _filmRepository = filmRepository;
            _validator=validator;
        }





        #region AddFilm

        /// <summary>
        ///  Adds a new film to the collection.
        /// </summary>
        /// <param name="film"></param>
        /// <returns></returns>


        [HttpPost("post")]
        public async Task<IActionResult> AddFilm(FilmDTO film)
        {
            var validationResult = _validator.Validate(film);
           
            if (!validationResult.IsValid)
            {
                return BadRequest(validationResult.Errors);
            }

            await _filmRepository.AddFilm(film);
            return Ok("Record Created Successfully");

        }
        #endregion

       

        #region SearchFilmsByTitle

        /// <summary>
        /// Searches for films in the collection by their title.
        /// </summary>
        /// <param name="title"></param>
        /// <returns></returns>

        [HttpGet("title/{title}")]

        public async Task<IActionResult> SearchFilmsByTitle(string title)
        {
            try
            {
                List<FilmDTO> films = await _filmRepository.SearchFilmsByTitle(title);
                return Ok(films);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        #endregion


        #region SearchFilmsByReleaseYear

        /// <summary>
        /// Searches for films in the collection by their release year.
        /// </summary>
        /// <param name="releaseYear"></param>
        /// <returns></returns>

        [HttpGet("year/{releaseYear}")]

        public async Task<IActionResult> SearchFilmsByReleaseYear(string releaseYear)
        { 
            try
            {
                List<FilmDTO> films = await _filmRepository.SearchFilmsByReleaseYear(releaseYear);
                return Ok(films);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        #endregion


        #region SearchFilmsByRentalDurationGreater

        /// <summary>
        /// Searches for films in the collection with a rental duration greater than the specified value.
        /// </summary>
        /// <param name="rentalDuration"></param>
        /// <returns></returns>
        /// 

        [HttpGet("duration/gt/{rentalDuration}")]
        public async Task<IActionResult> SearchFilmsByRentalDurationGreater(byte rentalDuration)
        {
            try
            {
                List<FilmDTO> films = await _filmRepository.SearchFilmsByRentalDurationGreater(rentalDuration);
                return Ok(films);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        #endregion

        #region SearchFilmsByRentalRateGreater

        /// <summary>
        /// Searches for films in the collection with a rental rate greater than the specified value.
        /// </summary>
        /// <param name="rentalRate"></param>
        /// <returns></returns>

        [HttpGet("rate/gt/{rentalRate}")]

        public async Task<IActionResult> SearchFilmsByRentalRateGreater(decimal rentalRate)
        {
            try
            {
                List<FilmDTO> films = await _filmRepository.SearchFilmsByRentalRateGreater(rentalRate);
                return Ok(films);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


        #endregion


        #region SearchFilmsByLengthGreater

        /// <summary>
        /// Searches for films in the collection with a length greater than the specified value.
        /// </summary>
        /// <param name="length"></param>
        /// <returns></returns>

        [HttpGet("length/gt/{length}")]
        public async Task<IActionResult> SearchFilmsByLengthGreater(short length)
        {
            try
            {
                List<FilmDTO> films = await _filmRepository.SearchFilmsByLengthGreater(length);
                return Ok(films);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


        #endregion


        #region SearchFilmsByRentalDurationLower

        /// <summary>
        /// Searches for films in the collection with a rental duration lower than the specified value.
        /// </summary>
        /// <param name="rentalDuration"></param>
        /// <returns></returns>

        [HttpGet("duration/lt/{rentalDuration}")]
        public async Task<IActionResult> SearchFilmsByRentalDurationLower(byte rentalDuration)
        {
            try
            {
                List<FilmDTO> films = await _filmRepository.SearchFilmsByRentalDurationLower(rentalDuration);
                return Ok(films);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        #endregion



        #region SearchFilmsByRentalRateLower

        /// <summary>
        /// Searches for films in the collection with a rental rate lower than the specified value.
        /// </summary>
        /// <param name="rentalRate"></param>
        /// <returns></returns>

        [HttpGet("rate/lt/{rentalRate}")]

        public async Task<IActionResult> SearchFilmsByRentalRateLower(decimal rentalRate)
        {
            try
            {
                List<FilmDTO> films = await _filmRepository.SearchFilmsByRentalRateLower(rentalRate);
                return Ok(films);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        #endregion


        #region SearchFilmsByLengthLower

        /// <summary>
        /// Searches for films in the collection with a length lower than the specified value.
        /// </summary>
        /// <param name="length"></param>
        /// <returns></returns>

        [HttpGet("length/lt/{length}")]
        public async Task<IActionResult> SearchFilmsByLengthLower(short length)
        {
            try
            {
                List<FilmDTO> films = await _filmRepository.SearchFilmsByLengthLower(length);
                return Ok(films);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        #endregion

        #region SearchFilmsByReleaseYearRange


        /// <summary>
        /// Searches for films in the collection within the specified release year range.
        /// </summary>
        /// <param name="from"></param>
        /// <param name="to"></param>
        /// <returns></returns>

        [HttpGet("betweenyear/{from}/{to}")]

        public async Task<IActionResult> SearchFilmsByReleaseYearRange(int from, int to)
        {
            try
            {
                List<FilmDTO> films = await _filmRepository.SearchFilmsByReleaseYearRange(from,to);
                return Ok(films);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        #endregion


        #region  SearchFilmsByRatingLower

        /// <summary>
        /// Searches for films in the collection with a rating lower than the specified value.
        /// </summary>
        /// <param name="rating"></param>
        /// <returns></returns>

        [HttpGet("rating/lt/{rating}")]
        public async Task<IActionResult> SearchFilmsByRatingLower(string rating)
        {
            try
            {
                List<FilmDTO> films = await _filmRepository.SearchFilmsByRatingLower(rating);
                return Ok(films);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        #endregion

        #region  SearchFilmsByRatingHigher

        /// <summary>
        /// Searches for films in the collection with a rating higher than the specified value.
        /// </summary>
        /// <param name="rating"></param>
        /// <returns></returns>

        [HttpGet("rating/gt/{rating}")]
        public async Task<IActionResult> SearchFilmsByRatingHigher(string rating)
        {
            try
            {
                List<FilmDTO> films = await _filmRepository.SearchFilmsByRatingHigher(rating);
                return Ok(films);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        #endregion

        #region SearchFilmsByLanguage

        /// <summary>
        /// Searches for films in the collection based on the specified language.
        /// </summary>
        /// <param name="language"></param>
        /// <returns></returns>

        [HttpGet("language/{language}")]

        public async Task<IActionResult> SearchFilmsByLanguage(string language)
        {
            try
            {
                List<FilmDTO> films = await _filmRepository.SearchFilmsByLanguage(language);
                return Ok(films);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        #endregion

        #region GetFilmCountByReleaseYear

        /// <summary>
        /// Returns the count of films in the collection for a specified release year.
        /// </summary>
        /// <returns></returns>

        [HttpGet("countbyyear")]
        public async Task<IActionResult> GetFilmCountByReleaseYear()
        {
            try
            {
                Dictionary<string, int> result = await _filmRepository.GetFilmCountByReleaseYear();
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        #endregion

        #region GetActorsByFilmId

        /// <summary>
        /// Retrieves the list of actors associated with a specific film using its ID.
        /// </summary>
        /// <param name="filmId"></param>
        /// <returns></returns>

        [HttpGet("{filmId}/actors")]
        public async Task<IActionResult> GetActorsByFilmId(int filmId)
        {
            try
            {
                List<ActorDTO> actors = await _filmRepository.GetActorsByFilmId(filmId);
                return Ok(actors);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        #endregion

        #region GetFilmsByCategory

        /// <summary>
        /// Retrieves a list of films from the collection that belong to a specified category.
        /// </summary>
        /// <param name="category"></param>
        /// <returns></returns>

        [HttpGet("category/{category}")]
        public async Task<IActionResult> GetFilmsByCategory(string category)
        {
            try
            {
                List<FilmDTO> films=await _filmRepository.GetFilmsByCategory(category);
                return Ok(films);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


        #endregion

        #region AssignActorToFilm

        /// <summary>
        /// Assigns an actor to a specific film in the collection.
        /// </summary>
        /// <param name="filmid"></param>
        /// <param name="actorId"></param>
        /// <returns></returns>

        [HttpPut("{filmid}/actor")]
        public async Task<IActionResult> AssignActorToFilm(int filmid, int actorId)
        {
            try
            {
                
                List<ActorDTO> actors=await _filmRepository.AssignActorToFilm(filmid, actorId);

                return Ok(actors);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        #endregion

        #region UpdateTitleOfFilm

        /// <summary>
        /// Updates the title of a specific film in the collection.
        /// </summary>
        /// <param name="filmid"></param>
        /// <param name="title"></param>
        /// <returns></returns>

        [HttpPut("update/title/{filmid}")]
        public async Task<IActionResult> UpdateTitleOfFilm(int filmid, string title)
        {
            try
            {
                List<FilmDTO> res = await _filmRepository.UpdateTitleOfFilm(filmid, title);
                return Ok(res);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        #endregion

        #region UpdateReleaseYearOfFilm

        /// <summary>
        /// Updates the release year of a specific film in the collection.
        /// </summary>
        /// <param name="filmid"></param>
        /// <param name="releaseyear"></param>
        /// <returns></returns>

        [HttpPut("update/releaseyear/{filmid}")]
        public async Task<IActionResult> UpdateReleaseYearOfFilm(int filmid, string releaseyear)
        {
            try
            {
                List<FilmDTO> res=await _filmRepository.UpdateReleaseYearOfFilm(filmid,releaseyear);
                return Ok(res);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        #endregion

        #region UpdateRentalDurationOfFilm

        /// <summary>
        /// Updates the rental duration of a specific film in the collection.
        /// </summary>
        /// <param name="filmid"></param>
        /// <param name="rentalduration"></param>
        /// <returns></returns>

        [HttpPut("update/rentaldurtion/{filmid}")]
        public async Task<IActionResult> UpdateRentalDurationOfFilm(int filmid, byte rentalduration)
        {
            try
            {
                 List<FilmDTO> res=await _filmRepository.UpdateRentalDurationOfFilm(filmid, rentalduration);
                return Ok(res);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        #endregion

        #region UpdateRentalRateOfFilm

        /// <summary>
        /// Updates the rental rate of a specific film in the collection.
        /// </summary>
        /// <param name="filmid"></param>
        /// <param name="rentalrate"></param>
        /// <returns></returns>

        [HttpPut("update/rentalrate/{filmid}")]
        public async Task<IActionResult> UpdateRentalRateOfFilm(int filmid, decimal rentalrate)
        {
            try
            {
                List<FilmDTO> res=await _filmRepository.UpdateRentalRateOfFilm(filmid, rentalrate);
                return Ok(res);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        #endregion

        #region UpdateRatingOfFilm

        /// <summary>
        /// Updates the rating of a specific film in the collection.
        /// </summary>
        /// <param name="filmid"></param>
        /// <param name="rating"></param>
        /// <returns></returns>

        [HttpPut("update/rating/{filmid}")]
        public async Task<IActionResult> UpdateRatingOfFilm(int filmid, string rating)
        {
            var validRatings = new[] { "NC-17", "R", "PG-13", "PG", "G" };

            if (!validRatings.Contains(rating))
            {
                return BadRequest("Please provide a valid rating: NC-17, R, PG-13, PG, or G.");
            }

            try
            {
                List<FilmDTO> res =await _filmRepository.UpdateRatingOfFilm(filmid, rating);
                return Ok(res);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }



        #endregion


        #region UpdateLanguageOfFilm

        /// <summary>
        /// Updates the language of a specific film in the collection.
        /// </summary>
        /// <param name="filmid"></param>
        /// <param name="language"></param>
        /// <returns></returns>

        [HttpPut("update/language/{filmid}")]
        public async Task<IActionResult> UpdateLanguageOfFilm(int filmid, LanguageDTO language)
        {
            try
            {
                await _filmRepository.UpdateLanguageOfFilm(filmid, language);
                return Ok("Updated Successfully");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        #endregion

        #region UpdateCategoryOfFilm

        /// <summary>
        /// Updates the category of a specific film in the collection.
        /// </summary>
        /// <param name="filmid"></param>
        /// <param name="CategoryId"></param>
        /// <returns></returns>

        [HttpPut("update/category/{filmid}")]
        public async Task<IActionResult> UpdateCategoryOfFilm(int filmid, int CategoryId)
        {
            try
            {
                await _filmRepository.UpdateCategoryOfFilm(filmid,CategoryId);
                return Ok("Updated Successfully");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        #endregion





















    }
}
