namespace FilmRentalStore.DTO
{
    public class ActorDTO
    {
        public int ActorId { get; set; }

        public string FirstName { get; set; } = null!;

        public string LastName { get; set; } = null!;

        public DateTime LastUpdate { get; set; }
    }
}
