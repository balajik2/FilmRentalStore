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
                var topFilms = await _rentalRepository.GetTopTenRentedFilmsByStore(id);

                if (topFilms == null || !topFilms.Any())
                {
                    return NotFound("No rentals found for the given store.");
                 
                }

                return Ok(topFilms);
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
                var rentals = await _rentalRepository.GetCustomersWithDueRentalsByStore(id);

                if (rentals == null || !rentals.Any())
                {
                    return NotFound("No customers with due rentals found for the given store.");
                }

                return Ok(rentals); 
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("/api/rental/update/returndate/{id}")]

        public async Task<IActionResult> UpdateReturnDate(int id, [FromBody] DateTime returnDate)
        {
            try
            {
                var updatedRental = await _rentalRepository.UpdateReturnDate(id, returnDate);

                if (updatedRental == null)
                {
                    return NotFound( "Rental not found.");
                }

                return Ok(updatedRental);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

    }

}




