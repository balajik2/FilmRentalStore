using FilmRentalStore.DTO;
using FilmRentalStore.Services;
using FluentValidation;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

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






















    }
}
