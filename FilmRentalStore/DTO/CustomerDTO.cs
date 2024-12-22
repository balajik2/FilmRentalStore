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
    }
}
