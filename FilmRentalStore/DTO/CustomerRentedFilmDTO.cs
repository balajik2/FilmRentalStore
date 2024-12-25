namespace FilmRentalStore.DTO
{
    public class CustomerRentedFilmDTO
    {
        public int FilmId { get; set; }
        public decimal RentalRate { get; set; }
        public string Title { get; set; } = null!;

    }
}
