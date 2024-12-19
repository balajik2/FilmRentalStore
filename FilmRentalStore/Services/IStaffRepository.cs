using FilmRentalStore.DTO;
using Microsoft.AspNetCore.Mvc;

namespace FilmRentalStore.Services
{
    public interface IStaffRepository
    {
        Task AddStaff(StaffDTO staffDTO);
        Task<List<StaffDTO>> GetStaff();
        Task<List<StaffDTO>> GetStaffByLastName(string lastname);
        Task<List<StaffDTO>> GetStaffByFirstName(string firstname);
        Task<List<StaffDTO>> GetStaffByEmail(string email);
        Task<List<StaffDTO>> AssignAddress(StaffDTO staffDTO);
        Task<List<StaffDTO>> GetStaffByCity(string city);
        Task<List<StaffDTO>> GetStaffByCountry(string country);
        Task<List<StaffDTO>> GetStaffByPhoneNumber(string phone);
        Task<List<StaffDTO>> UpdateStaffByFirstName(int staffId, string newfirstname);
        Task<List<StaffDTO>> UpdateStaffByLastName(int staffId, string newlastname);
        Task<List<StaffDTO>> UpdateStaffByEmail(int staffId, string email);
        Task<List<StaffDTO>> AssignStoreToStaff(int staffId, int storeId);
        Task<List<StaffDTO>> UpdatePhoneNumberByStaff(int staffId, string newPhone);

    }
}
