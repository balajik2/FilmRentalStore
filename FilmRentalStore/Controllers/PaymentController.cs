using FilmRentalStore.DTO;
using FilmRentalStore.Models;
using FilmRentalStore.Services;
using FluentValidation;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace FilmRentalStore.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentController : ControllerBase
    {
        private readonly IPaymentRepository _context;
        private readonly IConfiguration _configuration;

        public PaymentController(IPaymentRepository payment, IConfiguration configuration)
        {
            _context = payment;
            _configuration = configuration;

        }


        #region Make Payment

        /// <summary> MakePayment
        /// 
        /// This method handles a PUT request to update a payment based on the given paymentId and amount.
        /// It first checks if the provided amount is positive; if not, it returns an error message saying "Amount must be a positive value.
        /// " If the amount is valid, it updates the payment record with the new amount and returns the updated result.
        /// In case of any errors during the update, it catches the exception and returns the error message.
        /// 
        /// </summary>
        /// <param name="paymentId"></param>
        /// <param name="amount"></param>
        /// <returns></returns>

        [HttpPut("MakePayment")]
        public async Task<IActionResult> MakePayment(int paymentId, decimal amount)
        {
            try
            {
                if (amount <= 0)
                {
                    return BadRequest("Amount must be a positive value.");
                }

                var result = await _context.MakePayment(paymentId, amount);
                return Ok(result);

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        #endregion


        #region GetCumulativeRevenueOfAllStores 

        /// <summary> GetCumulativeRevenueOfAllStores
        /// 
        /// This method handles a GET request to retrieve the cumulative revenue data of all stores.
        /// It first calls the service to fetch the revenue data.
        /// If no data is found, it returns a NotFound response with the message "No revenue data found.
        /// " If data is found, it returns the revenue data with a 200 OK response. 
        /// 
        /// </summary>
        /// <returns></returns>

        [HttpGet("GetCumulativeRevenueOfAllStores")]
        public async Task<IActionResult> GetCumulativeRevenueOfAllStores()
        {
            try
            {
                // Fetch the cumulative revenue data
                var revenueData = await _context.GetCumulativeRevenueOfAllStores();

                // Check if any data is returned
                if (revenueData == null || !revenueData.Any())
                {
                    return NotFound("No revenue data found.");
                }

                // Return the revenue data
                return Ok(revenueData);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        #endregion


        #region GetCumulativeRevenueForAStore

        /// <summary> GetCumulativeRevenueForAStore
        /// 
        /// This method handles a GET request to retrieve the cumulative revenue for a specific store based on the provided storeId.
        /// It calls the service to get the cumulative revenue data for the given store.
        /// If the operation is successful, it returns the result as a JSON response with a 200 OK status.
        /// 
        /// </summary>
        /// <param name="storeId"></param>
        /// <returns></returns>

        [HttpGet("GetCumulativeRevenueForAStore")]
        public async Task<IActionResult> GetCumulativeRevenueForAStore(int storeId)
        {
            try
            {
                var result = await _context.GetCumulativeRevenueForAStore(storeId);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        #endregion


        #region GetPaymentsByFilmTitle

        /// <summary> GetPaymentsByFilmTitle
        /// 
        /// This method handles a GET request to retrieve payment details based on the provided filmTitle.
        /// It first checks if the filmTitle is provided; if not, it returns a BadRequest with the message "Film title is required.
        /// " If a valid filmTitle is given, it calls the service to fetch the related payment details and returns the result as a JSON response with a success message, "Film details are in film table." 
        /// 
        /// </summary>
        /// <param name="filmTitle"></param>
        /// <returns></returns>

        [HttpGet("GetPaymentsByFilmTitle")]
        public async Task<IActionResult> GetPaymentsByFilmTitle(string filmTitle)
        {

            try
            {
                if (string.IsNullOrEmpty(filmTitle))
                {
                    return BadRequest("Film title is required.");
                }

                var result = await _context.GetPaymentsByFilmTitle(filmTitle);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        #endregion


        #region GetCumulativeRevenueStoreWise

        /// <summary> GetCumulativeRevenueStoreWise
        /// 
        /// This method handles a GET request to retrieve the cumulative revenue for a specific store, identified by the storeid.
        /// It first checks if the storeid corresponds to any existing payment data in the database.
        /// If no data is found for the given store, it returns a NotFound response with a message saying "No payments found for Store with ID {storeid}".
        /// If data is found, it returns the cumulative revenue details for that store as a JSON response.
        /// 
        /// </summary>
        /// <param name="storeid"></param>
        /// <returns></returns>

        [HttpGet("GetCumulativeRevenueStoreWise")]
        public async Task<IActionResult> GetCumulativeRevenueStoreWise(int storeid)
        {
            try
            {
                var payments = await _context.GetCumulativeRevenueStoreWise(storeid);

                if (payments == null || payments.Count == 0)
                {
                    return NotFound($"No payments found for Store with ID {storeid}.");
                }

                return Ok(payments);
            }
            catch (Exception ex)
            {
                return BadRequest($"Error occurred: {ex.Message}");
            }
        }


        #endregion


        #region GetCumulativeRevenueAllFilmsByStore

        /// <summary> GetCumulativeRevenueAllFilmsByStore
        /// 
        /// This method handles a GET request to retrieve the cumulative revenue for all films across all stores.
        /// It calls a service method to calculate the cumulative revenue, grouped by both film and store.
        /// If no data is found or if the result is empty, it returns a NotFound response with a message saying "No revenue data found for any film across stores.
        /// " If data is found, it returns the list of cumulative revenue for each film and its corresponding store as a JSON response.
        /// 
        /// </summary>
        /// <returns></returns>

        [HttpGet("GetCumulativeRevenueAllFilmsByStore")]
        public async Task<IActionResult> GetCumulativeRevenueAllFilmsByStore()
        {
            try
            {
                var result = await _context.GetCumulativeRevenueAllFilmsByStore();

                if (result == null || result.Count == 0)
                {
                    return NotFound("No revenue data found for any film across stores.");
                }

                return Ok(result);  
            }
            catch (Exception ex)
            {
                return BadRequest($"Error occurred: {ex.Message}");
            }
        }


        #endregion

    }
}
