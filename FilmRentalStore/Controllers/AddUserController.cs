using FilmRentalStore.DTO;
using FilmRentalStore.Services;
using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace FilmRentalStore.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AddUserController : ControllerBase
    {
        private readonly IAdduserRepository _adduserRepository;

        public AddUserController(IAdduserRepository adduserRepository)
        {
            _adduserRepository = adduserRepository;
            
        }

       
        [HttpPost("post")]
        public async Task<IActionResult> AddUser(AddUserDTO user)
        {

            await _adduserRepository.AddUser(user);
            return Ok("Created successfull !");

        }
    }

    
}
