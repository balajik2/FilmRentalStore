namespace FilmRentalStore.DTO
{
    public class PaymentWithFilmDTO
    {
        public int FilmId { get; set; }

        public string Title { get; set; } = null!;

        


        public int PaymentId { get; set; }

        public int CustomerId { get; set; }

        public int StaffId { get; set; }

        public int? RentalId { get; set; }

        public decimal Amount { get; set; }

        public DateTime PaymentDate { get; set; }

        public DateTime LastUpdate { get; set; }
    }
}
