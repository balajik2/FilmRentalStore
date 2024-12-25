﻿using FilmRentalStore.DTO;
using Microsoft.AspNetCore.Mvc;

namespace FilmRentalStore.Services
{
    public interface IActorRepository
    {
        Task<bool> AddActor(ActorDTO actorDto);

        Task<ActorDTO> GetActorsByLastName(string lastName);

        Task<ActorDTO> GetActorsByFirstName(string firstName);

        Task<List<ActorDTO>> UpdateLastNameById(int id, string lastname);
        Task<List<ActorDTO>> UpdateFirstNameById(int id, string firstname);


        Task<List<FilmDTO>> GetFilmsByActorId(int actorId);
        Task<List<FilmDTO>> AssignFilmToActor(int actorId, int filmId);
        Task<List<ActorDTO>> GetTopTenActorsByFilmCount();
    }
}
