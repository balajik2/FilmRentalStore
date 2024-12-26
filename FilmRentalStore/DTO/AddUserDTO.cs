namespace FilmRentalStore.DTO
{
    public class AddUserDTO
    {
        public int Id { get; set; }

        public string Username { get; set; } = null!;

        public string Password { get; set; } = null!;
    }
}
