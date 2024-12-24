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
        /// /Retrieves a list of distinct films rented by a specific customer, logs the SQL query, and maps the result to a list of RentalDTO
        /// </summary>
        /// <param name="customerId"></param>
        /// <returns></returns>
        public async Task<List<RentalDTO>> GetFilmsRentedByCustomer(int customerId)
        {
            var rentalsQuery = from customer in _context.Customers
                               join rental in _context.Rentals on customer.CustomerId equals rental.CustomerId
                               join inventory in _context.Inventories on rental.InventoryId equals inventory.InventoryId
                               join film in _context.Films on inventory.FilmId equals film.FilmId
                               where customer.CustomerId == customerId
                               select new 
                               {
                                   rental.RentalId,
                                   rental.RentalDate,
                                   rental.InventoryId,
                                   rental.CustomerId,
                                   rental.ReturnDate,
                                   rental.StaffId,
                                   rental.LastUpdate,
                                   film.Title
                               };

            
            var rentals = await rentalsQuery.Distinct().ToListAsync();

            var rentalDTOs = rentals.Select(r => new RentalDTO
            {
                RentalId = r.RentalId,
                RentalDate = r.RentalDate,
                InventoryId = r.InventoryId,
                CustomerId = r.CustomerId,
                ReturnDate = r.ReturnDate,
                StaffId = r.StaffId,
                LastUpdate = r.LastUpdate
            }).ToList();

            return rentalDTOs;
        }

        #endregion


        #region GetTopTenRentedFilms
        /// <summary>
        /// /Retrieves the top 10 most rented films for a specific store, grouping by film and handling cases where no rentals exist, then maps the results to a list of RentalDTO.
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


        #region GetTopTenRentedFilmsByStoreAsync
        /// <summary>
        /// Retrieves the top 10 most rented films for a specific store, groups by film, handles cases with no rentals, and maps the results to a list of RentalDTO.
        /// </summary>
        /// <param name="storeId"></param>
        /// <returns></returns>
        /// <exception cref="KeyNotFoundException"></exception>
        public async Task<List<RentalDTO>> GetTopTenRentedFilmsByStoreAsync(int storeId)
        {
            try
            {
                var topFilms = await (from rental in _context.Rentals
                                      join inventory in _context.Inventories on rental.InventoryId equals inventory.InventoryId
                                      join film in _context.Films on inventory.FilmId equals film.FilmId
                                      join customer in _context.Customers on rental.CustomerId equals customer.CustomerId
                                      where customer.StoreId == storeId
                                      select new
                                      {
                                          rental,
                                          film
                                      }).ToListAsync();

                if (!topFilms.Any())
                {
                    throw new KeyNotFoundException("No rentals found for the given store.");
                }

                var filteredFilms = topFilms
                    .GroupBy(x => new { x.film.FilmId, x.film.Title })
                    .OrderByDescending(g => g.Count())
                    .Take(10)
                    .Select(g => new RentalDTO
                    {
                        RentalId = g.First().rental.RentalId,
                     
                        InventoryId = g.First().rental.InventoryId,
                        CustomerId = g.First().rental.CustomerId,
                       
                        StaffId = g.First().rental.StaffId,
                        LastUpdate = g.First().rental.LastUpdate 
                    })
                    .ToList();

                return filteredFilms;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                throw;
            }
        }

        #endregion


        #region GetCustomersWithDueRentalsByStoreAsync
        /// <summary>
        /// Retrieves a list of customers with overdue rentals for a specific store, mapping the results to a list of RentalDTO.
        /// </summary>
        /// <param name="storeId"></param>
        /// <returns></returns>
        /// <exception cref="KeyNotFoundException"></exception>
        public async Task<List<RentalDTO>> GetCustomersWithDueRentalsByStoreAsync(int storeId)
        {
            try
            {
                
                var rentalsWithDueFilms = await (from rental in _context.Rentals
                                                 join inventory in _context.Inventories on rental.InventoryId equals inventory.InventoryId
                                                 join film in _context.Films on inventory.FilmId equals film.FilmId
                                                 join customer in _context.Customers on rental.CustomerId equals customer.CustomerId
                                                 where customer.StoreId == storeId && rental.ReturnDate == null
                                                 select new RentalDTO
                                                 {
                                                     RentalId = rental.RentalId,
                                                     RentalDate = rental.RentalDate,
                                                     InventoryId = rental.InventoryId,
                                                     CustomerId = rental.CustomerId,
                                                     ReturnDate = rental.ReturnDate,
                                                     StaffId = rental.StaffId,
                                                     LastUpdate = rental.LastUpdate
                                                 }).ToListAsync();

                if (!rentalsWithDueFilms.Any())
                {
                    throw new KeyNotFoundException("No customers with due rentals found for the given store.");
                }

                return rentalsWithDueFilms;
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
        /// Updates the return date of a rental based on the rental ID, saves the changes, and returns a success status.
        /// </summary>
        /// <param name="rentalId"></param>
        /// <param name="returnDate"></param>
        /// <returns></returns>
        public bool UpdateReturnDate(int rentalId, DateTime returnDate)
        {
            var rental = _context.Rentals.FirstOrDefault(r => r.RentalId == rentalId);
            if (rental == null)
            {
                return false; 
            }

            rental.ReturnDate = returnDate;
            rental.LastUpdate = DateTime.Now; 
            _context.SaveChanges();
            return true;
        }
        #endregion




    }
}