using FilmRentalStore.DTO;
using FilmRentalStore.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FilmRentalStore.Services
{
    public interface IRentalRepository
    {
        
         Task<RentalDTO> RentFilm(RentalDTO rentalDTO);

        Task<List<FilmDTO>> GetFilmsRentedByCustomer(int customerId);
        Task<List<Top10RentedFilmDTO>> GetTopTenRentedFilms();
        Task<List<Top10RentedFilmDTO>> GetTopTenRentedFilmsByStore(int storeId);
        Task<List<CustomerDTO>> GetCustomersWithDueRentalsByStore(int storeId);
        Task<RentalDTO> UpdateReturnDate(int rentalId, DateTime returnDate);


        




    }
}
