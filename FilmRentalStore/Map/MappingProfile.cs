using AutoMapper;
using FilmRentalStore.DTO;
using FilmRentalStore.Models;


namespace FilmRentalStore.Map
{
    public class MappingProfile : Profile
    {
        public MappingProfile() 
        {
            CreateMap<Film, FilmDTO>().ReverseMap();
            CreateMap<Actor, ActorDTO>().ReverseMap();
        }
    }
}
