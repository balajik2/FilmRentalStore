namespace FilmRentalStore.DTO
{
    public class PaymentDTO
    {
        public int PaymentId { get; set; }

        public int CustomerId { get; set; }

        public int StaffId { get; set; }

        public int? RentalId { get; set; }

        public decimal Amount { get; set; }

        public DateTime PaymentDate { get; set; }

        public DateTime LastUpdate { get; set; }
    }
}
