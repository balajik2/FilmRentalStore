﻿using AutoMapper;
using FilmRentalStore.DTO;
using FilmRentalStore.Models;

namespace FilmRentalStore.Map
{

    public class MappingProfile : Profile
    {
        public MappingProfile()
        {

            CreateMap<Store, StoreDTO>().ReverseMap();
            CreateMap<Address, AddressDTO>().ReverseMap();
            CreateMap<Staff, StaffDTO>().ReverseMap();
            CreateMap<Inventory, InventoryDTO>().ReverseMap();
            CreateMap<Customer, CustomerDTO>().ReverseMap();
            CreateMap<Payment, PaymentDTO>().ReverseMap();

            CreateMap<Rental, RentalDTO>().ReverseMap(); 
            CreateMap<Customer ,CustomerDTO>().ReverseMap();
            CreateMap<Actor, ActorDTO>().ReverseMap();

            CreateMap<Film, FilmDTO>().ReverseMap();
            CreateMap<Ruser,AddUserDTO>().ReverseMap();
        }

    }
}
