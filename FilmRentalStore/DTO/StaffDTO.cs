namespace FilmRentalStore.DTO
{
    public class StaffDTO
    {

        
            public int StaffId { get; set; }

            public string FirstName { get; set; } = null!;

            public string LastName { get; set; } = null!;

            public int AddressId { get; set; }

            public string? Email { get; set; }

            public bool Active { get; set; }


            public DateTime LastUpdate { get; set; }

        public string? UrlPath { get; set; }

       

    }
}
