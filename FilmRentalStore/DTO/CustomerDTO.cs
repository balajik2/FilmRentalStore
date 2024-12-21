using FilmRentalStore.Models;

namespace FilmRentalStore.DTO
{
    public class CustomerDTO
    {
        public int CustomerId { get; set; }

        public int StoreId { get; set; }

        public string FirstName { get; set; } = null!;

        public string LastName { get; set; } = null!;

        public string? Email { get; set; }

        public int AddressId { get; set; }

        public string Active { get; set; } = null!;

        public DateTime CreateDate { get; set; }

        public DateTime LastUpdate { get; set; }
      
       // public AddressDTO Addressval { get; internal set; }


        //public string Address1 { get; set; } = null!;

        //public string? Address2 { get; set; }

        //public string District { get; set; } = null!;

        //public int CityId { get; set; }

        //public string? PostalCode { get; set; }

        //public string Phone { get; set; }// = null!;
    }
}
