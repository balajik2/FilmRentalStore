using AutoMapper;
using FilmRentalStore.DTO;
using FilmRentalStore.Models;

namespace FilmRentalStore.MAP
{
    public class MappingProfile:Profile
    {
        public MappingProfile() { 
        CreateMap<Store,StoreDTO>().ReverseMap();
            CreateMap<Address,AddressDTO>().ReverseMap();
            CreateMap<Staff,StaffDTO>().ReverseMap();
            CreateMap<Inventory,InventoryDTO>().ReverseMap();
            CreateMap<Customer, CustomerDTO>().ReverseMap();
        
        }
    }
}
