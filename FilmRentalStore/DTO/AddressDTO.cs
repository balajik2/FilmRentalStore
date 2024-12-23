<<<<<<< HEAD
﻿using FilmRentalStore.Models;
namespace FilmRentalStore.DTO
{
    public class AddressDTO
    {


=======
﻿namespace FilmRentalStore.DTO
{
    public class AddressDTO
    {
>>>>>>> origin/FilmRentalStore-4
        public int AddressId { get; set; }

        public string Address1 { get; set; } = null!;

        public string? Address2 { get; set; }

        public string District { get; set; } = null!;

        public int CityId { get; set; }

        public string? PostalCode { get; set; }

        public string Phone { get; set; } = null!;

        public DateTime LastUpdate { get; set; }
    }
}
