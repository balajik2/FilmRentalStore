using AutoMapper;
namespace FilmRentalStore.DTO
{
    public class StoreDTO
    {
        public int StoreId { get; set; }
        public int ManagerStaffId { get; set; }
        public int AddressId { get; set; }
        public DateTime LastUpdate { get; set; }
    }
}
