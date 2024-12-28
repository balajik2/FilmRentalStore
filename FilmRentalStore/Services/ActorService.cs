using AutoMapper;
using FilmRentalStore.DTO;
using FilmRentalStore.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.TagHelpers;
using Microsoft.EntityFrameworkCore;

namespace FilmRentalStore.Services
{
    public class ActorService : IActorRepository

    {
        private readonly Sakila12Context _context;
        private readonly IMapper _mapper;

        public ActorService(Sakila12Context context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        #region AddActor
        /// <summary>
        /// Adds a new actor to the database using the provided ActorDTO and returns a success status.
        /// </summary>
        /// <param name="actorDto"></param>
        /// <returns></returns>
        public async Task<bool> AddActor(ActorDTO actorDto)
        {
            try
            {

                var actor = new Actor
                {
                    FirstName = actorDto.FirstName,
                    LastName = actorDto.LastName,
                    LastUpdate = actorDto.LastUpdate
                };

                _context.Actors.Add(actor);
                await _context.SaveChangesAsync();

                return true;
            }
            catch (Exception ex)
            {

                Console.WriteLine($"Error occurred while adding actor: {ex.Message}");
                return false;
            }
        }
        #endregion

        #region  GetActorsByLastName
        /// <summary>
        /// Retrieves an actor by their last name or first name from the database, mapping the result to an ActorDTO.
        /// </summary>
        /// <param name="lastName"></param>
        /// <returns></returns>
        public async Task<List<ActorDTO>> GetActorsByLastName(string lastName)
        {
            // Retrieve the list of actors matching the given first name
            var actorList = await _context.Actors
                .Where(c => c.LastName == lastName)
                .ToListAsync();

            // If no actors are found, return an empty list
            if (actorList == null || actorList.Count == 0)
            {
                return new List<ActorDTO>();
            }

            // Map the list of actors to a list of ActorDTO
            return _mapper.Map<List<ActorDTO>>(actorList);
        }
        #endregion

        #region  GetActorsByFirstName
        /// <summary>
        /// Retrieves an actor by their first name from the database and maps the result to an ActorDTO.
        /// </summary>
        /// <param name="firstName"></param>
        /// <returns></returns>
        public async Task<List<ActorDTO>> GetActorsByFirstName(string firstName)
        {
            // Retrieve the list of actors matching the given first name
            var actorList = await _context.Actors
                .Where(c => c.FirstName == firstName)
                .ToListAsync();

            // If no actors are found, return an empty list
            if (actorList == null || actorList.Count == 0)
            {
                return new List<ActorDTO>();
            }

            // Map the list of actors to a list of ActorDTO
            return _mapper.Map<List<ActorDTO>>(actorList);
        }
        #endregion

        #region UpdateLastNameById
        /// <summary>
        /// Updates the last name of an actor by their ID and returns the updated list of actors as ActorDTOs.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="lastname"></param>
        /// <returns></returns>
        /// <exception cref="KeyNotFoundException"></exception>
        public async Task<List<ActorDTO>> UpdateLastNameById(int id, string lastname)
        {
            var existingUser = await _context.Actors.FindAsync(id);

            if (existingUser == null)
            {
                throw new KeyNotFoundException($"Actor with ID {id} not found");
            }

            existingUser.LastName = lastname;

            await _context.SaveChangesAsync();

            var updatedList = await _context.Actors.Where(s => s.ActorId == existingUser.ActorId).ToListAsync();
            var result = _mapper.Map<List<ActorDTO>>(updatedList);
            return result;
        }
        #endregion

        #region UpdateFirstNameById
        /// <summary>
        /// Updates the first name of an actor by their ID and returns the updated list of actors as ActorDTOs.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="firstname"></param>
        /// <returns></returns>
        /// <exception cref="KeyNotFoundException"></exception>
        public async Task<List<ActorDTO>> UpdateFirstNameById(int id, string firstname)
        {
            var existingUser = await _context.Actors.FindAsync(id);

            if (existingUser == null)
            {
                throw new KeyNotFoundException($"Actor with ID {id} not found");
            }

            existingUser.FirstName = firstname;

            await _context.SaveChangesAsync();

            var updatedList = await _context.Actors.Where(s => s.ActorId == existingUser.ActorId).ToListAsync();
            var result = _mapper.Map<List<ActorDTO>>(updatedList);
            return result;
        }
        #endregion


        #region  GetFilmsByActorId
        /// <summary>
        /// Retrieves a list of films associated with a specific actor by their ID and maps the result to a list of FilmDTOs.
        /// </summary>
        /// <param name="actorId"></param>
        /// <returns></returns>
        /// <exception cref="KeyNotFoundException"></exception>
        public async Task<List<FilmDTO>> GetFilmsByActorId(int actorId)
        {
            
            var existingActor = await _context.Actors.FindAsync(actorId);

            if (existingActor == null)
            {
                throw new KeyNotFoundException($"Actor with ID {actorId} not found");
            }

           
            var relatedFilms = await _context.FilmActors
                .Where(fa => fa.ActorId == actorId)
                .Select(fa => fa.Film)
                .ToListAsync();

            
            var result = _mapper.Map<List<FilmDTO>>(relatedFilms);

            return result;
        }
        #endregion


        #region  AssignFilmToActor
        /// <summary>
        /// Assigns a film to an actor by their respective IDs and returns the updated list of films as FilmDTOs.
        /// </summary>
        /// <param name="actorId"></param>
        /// <param name="filmId"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public async Task<List<FilmDTO>> AssignFilmToActor(int actorId, int filmId)
        {
            var actor = await _context.Actors.FindAsync(actorId);
            if (actor == null)
            {
                throw new Exception("Actor not found");
            }
            var film = await _context.Films.FindAsync(filmId);
            if (film == null)
            {
                throw new Exception("Film not found");
            }
            var filmActor = new FilmActor
            {
                ActorId = actorId,
                FilmId = filmId,
        
                LastUpdate = DateTime.UtcNow
            };

            _context.FilmActors.Add(filmActor);
            await _context.SaveChangesAsync();

            var updatedlist = await _context.Films.Where(s => s.FilmId == film.FilmId).Include(s => s.FilmActors).ToListAsync();

            return _mapper.Map<List<FilmDTO>>(updatedlist);
        }
        #endregion


        #region GetTopTenActorsByFilmCount
        /// <summary>
        /// Retrieves the top 10 actors based on the number of films they have appeared in, and returns the results as a list of Top10ActorByFilmDTO.
        /// </summary>
        /// <returns></returns>
        public async Task<List<Top10ActorByFilmDTO>> GetTopTenActorsByFilmCount()
        {

            var updatedList = await _context.Actors
                .Join(
                    _context.FilmActors,
                    actor => actor.ActorId,
                    filmActor => filmActor.ActorId,
                    (actor, filmActor) => new { actor, filmActor }
                )
                .GroupBy(joined => new
                {
                    joined.actor.ActorId,
                    joined.actor.FirstName,
                    joined.actor.LastName

                })
                .Select(group => new
                {
                    group.Key.ActorId,
                    group.Key.FirstName,
                    group.Key.LastName,

                    FilmCount = group.Count()
                })
                .OrderByDescending(group => group.FilmCount)
                .Take(10)
                .ToListAsync();


            var result = updatedList.Select(actor => new Top10ActorByFilmDTO
            {
                ActorId = actor.ActorId,
                FirstName = actor.FirstName,
                LastName = actor.LastName,
                NoOfFilm = actor.FilmCount

            }).ToList();


            return result;
        }

        #endregion

    }
}

