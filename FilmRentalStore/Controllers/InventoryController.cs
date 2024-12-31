using FilmRentalStore.DTO;
using FilmRentalStore.Models;
using FilmRentalStore.Services;
using FluentValidation;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using AutoMapper;

namespace FilmRentalStore.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class InventoryController : ControllerBase
    {
        private readonly Sakila12Context _context;
        private readonly IInventoryRepository _InventoryRepository;
        private readonly IValidator<InventoryDTO> _validator;
        public InventoryController(IInventoryRepository inventoryRepository, IValidator<InventoryDTO> validator)
        {
            _InventoryRepository = inventoryRepository;
            _validator = validator;
        }

        [HttpPost("add")]

        public async Task<IActionResult> AddFilm(InventoryDTO inventoryDTO)
        {
         
            var validatorResult = _validator.Validate(inventoryDTO);
            if (!validatorResult.IsValid)
            {
                return BadRequest(validatorResult.Errors);
            }
            await _InventoryRepository.AddFilm(inventoryDTO);

            return Ok("Record created successfully");

        }
        [HttpGet("Count")]
        public async Task<IActionResult> CountOfFilmasync()
        {
            try
            {
                var inventory = await _InventoryRepository.CountOfFilmasync();
                return Ok(inventory);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }



        [HttpGet("Films")]
        public async Task<IActionResult>  GetAllFilmsinaStore(int storeid)
        {
            try
            {
                var data = await _InventoryRepository.GetAllFilmsinaStore(storeid);
                return Ok(data);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpGet("Film{filmid}")]
        public async Task<IActionResult> Getinventoryforallfilms(int filmid)
        {
            try
            {
                var data = await _InventoryRepository.Getinventoryforallfilms(filmid);
                return Ok(data);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpGet("film/{filmid}/store/{storeid}")]
        public async Task<IActionResult> GetFilmCountInStore(int filmid,int storeid)
        {
            try
            {
                var data = await _InventoryRepository.GetFilmCountInStore(filmid,storeid);
                return Ok(data);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
