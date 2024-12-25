namespace FilmRentalStore.DTO
{
    public class FilmDTO
    {
        public int FilmId { get; set; }

        public string Title { get; set; } = null!;

        public string? Description { get; set; }

        public string? ReleaseYear { get; set; }

        public int LanguageId { get; set; }

        public int? OriginalLanguageId { get; set; }

        public byte RentalDuration { get; set; }

        public decimal RentalRate { get; set; }

        public short? Length { get; set; }

        public decimal ReplacementCost { get; set; }


        public DateTime LastUpdate { get; set; }
    }
}
