using AutoMapper;
using FilmRentalStore.DTO;
using FilmRentalStore.Models;

namespace FilmRentalStore.Map
{
<<<<<<< HEAD
    public class MappingProfile:Profile
    {
        public MappingProfile() { 
        CreateMap<Store,StoreDTO>().ReverseMap();
            CreateMap<Address,AddressDTO>().ReverseMap();
            CreateMap<Staff,StaffDTO>().ReverseMap();
            CreateMap<Inventory,InventoryDTO>().ReverseMap();
            CreateMap<Customer, CustomerDTO>().ReverseMap();
        
=======
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Staff, StaffDTO>().ReverseMap();
            CreateMap<Payment, PaymentDTO>().ReverseMap();
>>>>>>> origin/FilmRentalStore-5
        }
    }
}
