namespace FilmRentalStore.DTO
{
    public class CategoryDTO
    {
        public int CategoryId { get; set; }

        public string Name { get; set; } = null!;

        public DateTime LastUpdate { get; set; }
    }
}
