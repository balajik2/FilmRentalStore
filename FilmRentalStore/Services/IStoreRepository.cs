using FilmRentalStore.DTO;
using FilmRentalStore.Models;
namespace FilmRentalStore.Services
{
    public interface IStoreRepository
    {

         Task AddStore(StoreDTO storeDto);

        Task<List<StoreDTO>> GetStoreByCity(string city);
        Task<List<StoreDTO>> GetStoreByCountry(string country);
       Task<List<StaffDTO>> GetAllStaffOfStore(int storeid);
        Task  AssignAddress(int storeid, AddressDTO addressDto);
        Task<List<CustomerDTO>> GetAllCustomers(int storeid);
        Task <StoreDTO> GetStorebyPhoneno(string phone);
        Task UpdatePhoneBystoreid(int storeid, string phone);

        Task AssignManager(int id, StaffDTO staff);
        Task<List<StaffDTO>> GetAllManagerOfStore(int storeid);
        Task<List<JoinDTO>> GetAllStaffAndStore();



    }
}
