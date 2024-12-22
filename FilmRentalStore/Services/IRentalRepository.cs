using FilmRentalStore.DTO;
using FilmRentalStore.Models;
using Microsoft.AspNetCore.Mvc;

namespace FilmRentalStore.Services
{
    public interface IRentalRepository
    {
        // Task AddRentalFilm(RentalDTO rentalDTO);
         Task<RentalDTO> RentFilm(RentalDTO rentalDTO);
       
        Task<List<RentalDTO>> GetFilmsRentedByCustomer(int  customerId);
        Task<List<RentalDTO>> GetTopTenRentedFilms();
        Task<List<RentalDTO>> GetTopTenRentedFilmsByStoreAsync(int storeId);
        Task<List<RentalDTO>> GetCustomersWithDueRentalsByStoreAsync(int storeId);

        public bool UpdateReturnDate(int rentalId, DateTime returnDate);

        


    }
}
