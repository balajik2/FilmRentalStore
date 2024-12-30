namespace FilmRentalStore.DTO
{
    public class Top10ActorByFilmDTO
    {
        public int ActorId { get; set; }

        public string FirstName { get; set; } = null!;

        public string LastName { get; set; } = null!;

        public int NoOfFilm { get; set; }

    }
}
