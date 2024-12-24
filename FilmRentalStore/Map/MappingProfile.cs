using AutoMapper;
using FilmRentalStore.DTO;
using FilmRentalStore.Models;

namespace FilmRentalStore.Map
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Rental, RentalDTO>().ReverseMap();
            CreateMap<Film, RentalDTO>().ForMember(dest => dest.RentalId, opt => opt.Ignore());
          
            CreateMap<Actor, ActorDTO>().ReverseMap();
            CreateMap<Film, FilmDTO>().ReverseMap();

        }

    }
} 