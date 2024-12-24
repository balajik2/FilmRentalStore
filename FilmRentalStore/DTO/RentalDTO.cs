using System.ComponentModel.DataAnnotations;

namespace FilmRentalStore.DTO
{
    public class RentalDTO
    {
      
       
        public int RentalId { get; set; }

        public DateTime RentalDate { get; set; }
       
        public int InventoryId { get; set; }
      
        public int CustomerId { get; set; }

        public DateTime? ReturnDate { get; set; }

        public int StaffId { get; set; }

        public DateTime LastUpdate { get; set; }
 
    }
}
