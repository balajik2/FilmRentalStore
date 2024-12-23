using FilmRentalStore.Models;

namespace FilmRentalStore.DTO
{
    public class InventoryDTO
    {
        public int InventoryId { get; set; }

        public int FilmId { get; set; }

        public int StoreId { get; set; }

        public DateTime LastUpdate { get; set; }

       
    }
}
