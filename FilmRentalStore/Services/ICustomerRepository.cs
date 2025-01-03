﻿using FilmRentalStore.DTO;
using FilmRentalStore.Models;
namespace FilmRentalStore.Services
{
    public interface ICustomerRepository
    {
        Task<CustomerDTO> AddCustomer(CustomerDTO customer);

        Task <List<CustomerDTO>> GetCustomer();

        Task<List<CustomerDTO>> GetCustomerById(int id);

        Task<List<CustomerDTO>> GetCustomerByLastName(string lastname);

        Task<List<CustomerDTO>> GetCustomerByFirstName(string firstname);

        Task<CustomerDTO> GetCustomerByEmail(string? email);

         Task<List<CustomerDTO>> AssignAddress(int id,int addressid);

       // Task UpdateCustomerById(int customerid, AddressDTO address);

        Task<List<CustomerwithAddressDTO>> GetCustomerByCity(string city);

        Task<List<CustomerwithAddressDTO>> GetCustomerByCountry(string city);

        Task<List<CustomerDTO>> GetActiveCustomers();

        Task<List<CustomerDTO>> GetInActiveCustomers();

        Task<List<CustomerwithAddressDTO>> GetCustomerByPhone(string phone);

       
        Task<List<CustomerDTO>> UpdateFirstNameById(int id, string name);

        Task<List<CustomerDTO>> UpdateLastNameById(int id, string lastname);

        Task<List<CustomerDTO>> UpdateEmailCustomer(int id, string email);



        Task<List<CustomerDTO>> UpdatePhoneCustomer(int id, string phone);

        Task<List<CustomerDTO>> AssignStoreToCustomer(int customerid, int storeid);
    }
}
