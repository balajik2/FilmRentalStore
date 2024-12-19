using AutoMapper;
using FilmRentalStore.DTO;
using FilmRentalStore.Model;

namespace FilmRentalStore.Map
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Staff, StaffDTO>().ReverseMap();
        }
    }
}
