namespace FilmRentalStore.DTO
{
    public class JoinDTO
    {
       

        public string FirstName { get; set; } = null!;

        public string LastName { get; set; } = null!;
        public string? Email { get; set; }
        public string Phone { get; set; } = null!;
        public string Address1 { get; set; } = null!;

        public string? Address2 { get; set; }
        public string City1 { get; set; } = null!;

    }
}
