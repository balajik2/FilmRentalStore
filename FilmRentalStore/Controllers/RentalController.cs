using FilmRentalStore.DTO;
using FilmRentalStore.Models;
using FilmRentalStore.Services;
using FluentValidation;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FilmRentalStore.Controllers
{
    public class RentalController : Controller
    {
        private readonly IRentalRepository _rentalRepository;
        private readonly IConfiguration _configuration;
        private readonly IValidator<RentalDTO> _validator;

        
        public RentalController(IRentalRepository rentalRepository, IConfiguration configuration, IValidator<RentalDTO> validator)
        {
            _rentalRepository = rentalRepository;
            _configuration = configuration;
            _validator = validator;


        }

       
        [HttpPost("/api/rental/add")]
        public async Task<IActionResult> RentFilm([FromBody] RentalDTO rentalDTO)
        {
            if (rentalDTO == null)
            {
                return BadRequest("Rental data is required.");
            }

            try
            {
                var createdRental = await _rentalRepository.RentFilm(rentalDTO);
                return Ok("Record Created Sucessfully");

            }
           
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


        [HttpGet("/api/rental/customer/{id}")]
        public async Task<IActionResult> GetFilmsRentedByCustomer(int id)
        {
            try
            {
                var films = await _rentalRepository.GetFilmsRentedByCustomer(id);
                return Ok(films);
            }
           
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpGet("/api/rental/toptenfilms")]
        public async Task<IActionResult> GetTopTenRentedFilms()
        {
            try
            {
                var films = await _rentalRepository.GetTopTenRentedFilms();
                return Ok(films);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpGet("/api/rental/toptenfilms/store/{id}")]
       
        public async Task<IActionResult> GetTopTenFilmsByStore(int id)
        {
            try
            {
                var topFilms = await _rentalRepository.GetTopTenRentedFilmsByStoreAsync(id);

                if (topFilms == null || !topFilms.Any())
                {
                    return NotFound(new
                    {
                        message = "No rentals found for the given store."
                    });
                }

                return Ok(new
                {
                    message = "Top 10 most rented films retrieved successfully.",
                    data = topFilms
                });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpGet("/api/rental/due/store/{id}")]
        
        public async Task<IActionResult> GetCustomersWithDueRentalsByStore(int id)
        {
            try
            {
                var rentals = await _rentalRepository.GetCustomersWithDueRentalsByStoreAsync(id);

                if (rentals == null || !rentals.Any())
                {
                    return NotFound(new { message = "No customers with due rentals found for the given store." });
                }

                return Ok(rentals); 
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("/api/rental/update/returndate/{id}")]

       
        public IActionResult UpdateReturnDate(int id, [FromBody] string returnDateString)
        {
            if (string.IsNullOrEmpty(returnDateString))
            {
                return BadRequest("Invalid return date.");
            }


            string dateFormat = "yyyy-MM-dd HH:mm:ss.fff";

            if (!DateTime.TryParseExact(returnDateString, dateFormat, null, System.Globalization.DateTimeStyles.None, out var returnDate) || returnDate <= DateTime.UtcNow)
            {
                return BadRequest("Invalid return date format. Correct format is 'yyyy-MM-dd HH:mm:ss.fff'.");
            }

            var result = _rentalRepository.UpdateReturnDate(id, returnDate);

            if (!result)
            {
                return NotFound("Rental not found.");
            }

            return Ok("Rental return date updated successfully.");
        }


    }

}




