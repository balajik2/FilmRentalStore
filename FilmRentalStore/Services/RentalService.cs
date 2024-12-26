using System;
using System.Linq;
using AutoMapper;
using FilmRentalStore.DTO;
using FilmRentalStore.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;

namespace FilmRentalStore.Services
{
    public class RentalService : IRentalRepository
    {
        private readonly Sakila12Context _context;
        private readonly IMapper _mapper;
        
        public RentalService(Sakila12Context context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        #region Add Rental Film
        /// <summary>
        /// This method, RentFilm, takes a RentalDTO object, maps it to a Rental entity, saves it to the database, and returns the saved entity mapped back to RentalDTO
        /// </summary>
        /// <param name="rentalDTO"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>




        public async Task<RentalDTO> RentFilm(RentalDTO rentalDTO)
        {
            if (rentalDTO == null)
            {
                throw new ArgumentNullException(nameof(rentalDTO), "Rental data cannot be null");
            }

          
            var rental = _mapper.Map<Rental>(rentalDTO);

           
            _context.Rentals.Add(rental);

            
            await _context.SaveChangesAsync();

           
            return _mapper.Map<RentalDTO>(rental);
            
        }



        #endregion


        #region GetFilmsRentedByCustomer
        /// <summary>
        /// /Retrieves a list of  films rented by a specific customer and maps the result to a list of FilmDTO
        /// </summary>
        /// <param name="customerId"></param>
        /// <returns></returns>

        public async Task<List<FilmDTO>> GetFilmsRentedByCustomer(int customerId)
        {
            var films = await (from customer in _context.Customers
                               join rental in _context.Rentals on customer.CustomerId equals rental.CustomerId
                               join inventory in _context.Inventories on rental.InventoryId equals inventory.InventoryId
                               join film in _context.Films on inventory.FilmId equals film.FilmId
                               where customer.CustomerId == customerId
                               select film).ToListAsync();


            var result = _mapper.Map<List<FilmDTO>>(films);

            return result;
        }

        #endregion


        #region GetTopTenRentedFilms
        /// <summary>
        /// /Retrieves the top 10 most rented films for a specific store, grouping by film and handling cases where no rentals exist, then maps the results to a list of Top10RentedFilmDTO.
        /// </summary>
        /// <returns></returns>

        public async Task<List<Top10RentedFilmDTO>> GetTopTenRentedFilms()
        {
            var topTenFilms = await (from rental in _context.Rentals
                                     join inventory in _context.Inventories on rental.InventoryId equals inventory.InventoryId
                                     join film in _context.Films on inventory.FilmId equals film.FilmId
                                     group rental by new { film.FilmId, film.Title } into filmGroup
                                     orderby filmGroup.Count() descending
                                     select new Top10RentedFilmDTO
                                     {
                                         FilmId = filmGroup.Key.FilmId,
                                         Title = filmGroup.Key.Title,
                                         RentalCount = filmGroup.Count()
                                     })
                                     .Take(10)
                                     .ToListAsync();

            return topTenFilms;
        }





        #endregion


        #region GetTopTenRentedFilmsByStore
        /// <summary>
        /// the code retrieves rental records, groups them by films, counts their rentals, sorts by popularity, takes the top 10, and 
        /// returns the data as a list of Top10RentedFilmDTO
        /// </summary>
        /// <param name="storeId"></param>
        /// <returns></returns>
        /// <exception cref="KeyNotFoundException"></exception>
        public async Task<List<Top10RentedFilmDTO>> GetTopTenRentedFilmsByStore(int storeId)
        {
            var topTenFilms = await (from rental in _context.Rentals
                                     join inventory in _context.Inventories on rental.InventoryId equals inventory.InventoryId
                                     join film in _context.Films on inventory.FilmId equals film.FilmId
                                     join customer in _context.Customers on rental.CustomerId equals customer.CustomerId
                                     where customer.StoreId == storeId 
                                     group rental by new { film.FilmId, film.Title } into filmGroup
                                     orderby filmGroup.Count() descending
                                     select new Top10RentedFilmDTO
                                     {
                                         FilmId = filmGroup.Key.FilmId,
                                         Title = filmGroup.Key.Title,
                                         RentalCount = filmGroup.Count()
                                     })
                                     .Take(10)
                                     .ToListAsync();

            return topTenFilms;
        }


        #endregion


        #region GetCustomersWithDueRentalsByStore
        /// <summary>
        /// Retrieves a list of customers with overdue rentals for a specific store, mapping the results to a list of CustomerDTO.
        /// </summary>
        /// <param name="storeId"></param>
        /// <returns></returns>
        /// <exception cref="KeyNotFoundException"></exception>
       
        public async Task<List<CustomerDTO>> GetCustomersWithDueRentalsByStore(int storeId)
        {
            try
            {
                var customersWithDueRentals = await (from rental in _context.Rentals
                                                     join inventory in _context.Inventories on rental.InventoryId equals inventory.InventoryId
                                                     join film in _context.Films on inventory.FilmId equals film.FilmId
                                                     join customer in _context.Customers on rental.CustomerId equals customer.CustomerId
                                                     where customer.StoreId == storeId && rental.ReturnDate == null
                                                     select customer).ToListAsync();

                if (!customersWithDueRentals.Any())
                {
                    throw new KeyNotFoundException("No customers with due rentals found for the given store.");
                }

                
                var result = _mapper.Map<List<CustomerDTO>>(customersWithDueRentals);

                return result;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                throw;
            }
        }


        #endregion

        #region  UpdateReturnDate
        /// <summary>
        /// Updates the return date of a rental based on the rental ID, saves the changes, and returns rental
        /// </summary>
        /// <param name="rentalId"></param>
        /// <param name="returnDate"></param>
        /// <returns></returns>

        public async Task<Rental> UpdateReturnDate(int rentalId, DateTime returnDate)
        {
            var rental =  _context.Rentals.FirstOrDefault(r => r.RentalId == rentalId);
            if (rental == null)
            {
                return null;
            }

            rental.ReturnDate = returnDate;
            rental.LastUpdate = DateTime.Now;
            await _context.SaveChangesAsync(); 
            return rental;
        }


        #endregion




    }
}