using System;
using System.ComponentModel.DataAnnotations;
using FilmRentalStore.DTO;
using FilmRentalStore.Models;
using FilmRentalStore.Services;
using FluentValidation;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;

namespace FilmRentalStore.Controllers
{
    public class ActorController : Controller
    {
        private readonly IActorRepository _actorRepository;

        private readonly IValidator<ActorDTO> _validator;
         

        public ActorController(IActorRepository actorRepository, IValidator<ActorDTO> validator)
        {
            _actorRepository = actorRepository;
        }
     

        [HttpPost("/api/actors/post")]
        public async Task<IActionResult> AddActor([FromBody] ActorDTO actorDto)
        {
            if (actorDto == null)
            {
                return BadRequest("Actor data is required.");
            }
            else if (string.IsNullOrEmpty(actorDto.FirstName) || string.IsNullOrEmpty(actorDto.LastName))
            {
                return BadRequest(new
                {
                    timeStamp = DateTime.Now.ToString("yyyy-MM-dd"),
                    message = "Validation failed: FirstName and LastName are required."
                });
            }
            try
            {
                var createdActor = await _actorRepository.AddActor(actorDto);
                return Ok("Record created successfully");
               
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpGet("/api/actors/lastname/{ln}")]

        public async Task<IActionResult> GetActorsByLastName(string ln)
        {
            var actors = await _actorRepository.GetActorsByLastName(ln);
            if (actors == null || actors.Count == 0)
            {
                return NotFound("No actors found with the given last name.");
            }
            return Ok(actors);
        }
        [HttpGet("/api/actors/firstname/{fn}")]
        public async Task<IActionResult> GetActorsByFirstName(string fn)
        {
            var actors = await _actorRepository.GetActorsByFirstName(fn);
            if (actors == null || actors.Count == 0)
            {
                return NotFound("No actors found with the given first name.");
            }
            return Ok(actors);
        }
        [HttpPut("/api/actors/update/lastname/{id}")]


        public async Task<IActionResult> UpdateLastNameById(int id, string name)
        {
            try
            {
                var result = await _actorRepository.UpdateLastNameById(id, name);
                return Ok(result);

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("/api/actors/update/firstname/{id}")]


        public async Task<IActionResult> UpdateFirstNameById(int id, string name)
        {
            try
            {
                var result = await _actorRepository.UpdateFirstNameById(id, name);
                return Ok(result);

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpGet("/api/actors/{id}/films")]
        public async Task<IActionResult> GetFilmsByActorId(int id)
        {
            try
            {
                
                var films = await _actorRepository.GetFilmsByActorId(id);

                
                if (films == null || films.Count == 0)
                {
                    return NotFound("No films found for the actor.");
                }

               
                return Ok(films);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                
                return BadRequest(ex.Message);
            }
        }
        [HttpPut("/api/actors/{id}/film")]
        public async Task<IActionResult> AssignFilmToActor(int id, int filmId)
        {
            try
            {
                
                var filmDTO = await _actorRepository.AssignFilmToActor(id, filmId);

                if (filmDTO == null || filmDTO.Count == 0)
                {
                    return NotFound("Failed to assign actor to the film.");
                }

                
                return Ok(filmDTO);
            }
            catch (Exception ex)
            {
               
                return BadRequest(ex.Message);
            }
        }
        [HttpGet("/api/actors/toptenbyfilmcount")]
        public async Task<IActionResult> GetTopTenActorsByFilmCount()
        {
            try
            {
                var topActors = await _actorRepository.GetTopTenActorsByFilmCount();

                if (topActors == null || topActors.Count == 0)
                {
                    return NotFound("No actors found.");
                }

                return Ok(topActors);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

    }


}

