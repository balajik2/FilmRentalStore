using FilmRentalStore.DTO;
using FilmRentalStore.Models;
using FilmRentalStore.Services;
using AutoMapper;
using FluentValidation;
using Microsoft.AspNetCore.Http;
using FilmRentalStore.Validators;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics.Metrics;
using Microsoft.EntityFrameworkCore;

namespace FilmRentalStore.Controllers
{
    [Route("api/Customers")]
    [ApiController]
    public class CustomerController : ControllerBase
    {


        private readonly ICustomerRepository _CustomerService;
        private readonly IValidator<CustomerDTO> _Customervalidator;

        public CustomerController(ICustomerRepository _customerService, IValidator<CustomerDTO> customervalidator)
        {
            _CustomerService = _customerService;
            _Customervalidator = customervalidator;
        }



        #region AddCustomer

        /// <summary>
        /// Add a new Customer to the Collection
        /// </summary>
        /// <param name="customer"></param>
        /// <returns></returns>

        [HttpPost]

        

        public async Task<IActionResult> AddCustomer(CustomerDTO customer)
        {
            try
            {
                var validateresult = _Customervalidator.Validate(customer);

                if (customer == null)
                {
                    return BadRequest();
                }
                if (!validateresult.IsValid)
                {
                    return BadRequest(validateresult.Errors);
                }

              var newcustomer =  await _CustomerService.AddCustomer(customer);
                 return CreatedAtAction("GetCustomer", new { id =newcustomer.CustomerId}, newcustomer);
                //return Ok(customervalue);
            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        #endregion


        #region GetCustomer


        /// <summary>
        /// Get the Collection of all customer details
        /// </summary>
        /// <returns></returns>

        [HttpGet("GetCustomer")]
        public async Task<IActionResult> GetCustomer()
        {
            try
            {
                var details = await _CustomerService.GetCustomer();
                return Ok(details);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        #endregion


        
        [HttpGet("customerbyid/{id}")]

        public async Task<IActionResult> GetCustomerById(int id)
        {
            try
            {
                List<CustomerDTO> cust = await _CustomerService.GetCustomerById(id);

                return Ok(cust);


            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


        #region GetCustomerByLastName

        /// <summary>
        /// Get the Customer details in the Collection by LastName
        /// </summary>
        /// <param name="lastname"></param>
        /// <returns></returns>
        /// 


        [HttpGet("lastname/{lastname}")]

        public async Task<IActionResult> GetCustomerByLastName(string lastname)
        {

            if (string.IsNullOrWhiteSpace(lastname))
            {
                return BadRequest("'lastname' is required.");
            }

            try
            {
                List<CustomerDTO> cust = await _CustomerService.GetCustomerByLastName(lastname);
              
                    return Ok(cust);
                
                
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        #endregion


        #region GetCustomerByFirstName

        /// <summary>
        /// Get the Customer details in the Collection by FirstName
        /// </summary>
        /// <param name="firstname"></param>
        /// <returns></returns>
        /// 

        [HttpGet("firstname/{firstname}")]

        public async Task<IActionResult> GetCustomerByFirstName(string firstname)
        {


            if (string.IsNullOrWhiteSpace(firstname))
            {
                return BadRequest("'firstName' is required.");
            }
            try
            {
                List<CustomerDTO> firstnameval = await _CustomerService.GetCustomerByFirstName(firstname);

                return Ok(firstnameval);


            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        #endregion


        #region GetCustomerByEmail

        /// <summary>
        /// Get the Collection of Customer Details by Email
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>

        [HttpGet("email/{email}")]
        public async Task<IActionResult> GetCustomerByEmail(string? email)
        {
            if (string.IsNullOrWhiteSpace(email))
            {
                return BadRequest("'email' is required.");
            }
            try
            {
               var supplier = await _CustomerService.GetCustomerByEmail(email);
                return Ok(supplier);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        #endregion
      

        #region AssignAddress

        /// <summary>
        /// Assigning Address to the customer in the collection
        /// </summary>
        /// <param name="customer"></param>
        /// <returns></returns>

        [HttpPut("AsignAddress")]
        public async Task<IActionResult> AssignAddress(int id,int addressid)
        {
            try
            {
                if (id == 0 || addressid == 0)
                {
                    return BadRequest("Customerid or store is Required");
                }
                var result = await _CustomerService.AssignAddress(id, addressid);
                return Ok(result);

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        #endregion



        #region GetCustomerByCity

        /// <summary>
        /// Get the Collection of Customer Details by City
        /// </summary>
        /// <param name="city"></param>
        /// <returns></returns>
        /// 
        [HttpGet("city/{city}")]


       
        public async Task<IActionResult> GetCustomerByCity(string city)
        {
            try
            {
                List<CustomerwithAddressDTO> cust = await _CustomerService.GetCustomerByCity(city);
                return Ok(cust);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        #endregion



        #region GetCustomerByCountry


        /// <summary>
        /// Get the Collection of Customer Details By Country
        /// </summary>
        /// <param name="country"></param>
        /// <returns></returns>
        /// 
        [HttpGet("country/{country}")]


       
        public async Task<IActionResult> GetCustomerByCountry(string country)
        {
            try
            {
                List<CustomerwithAddressDTO> cust = await _CustomerService.GetCustomerByCountry(country);
                return Ok(cust);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        #endregion


        #region GetActiveCustomers

        /// <summary>
        /// Get the collection of active customers
        /// </summary>
        /// <returns></returns>
        /// 
        [HttpGet("active")]
        public async Task<IActionResult> GetActiveCustomers()
        {
            try
            {
                List<CustomerDTO> cust = await _CustomerService.GetActiveCustomers();
                return Ok(cust);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }



        #endregion


        #region GetInActiveCustomers

        /// <summary>
        /// Get the Collection of inactive customers
        /// </summary>
        /// <returns></returns>
        /// 

        [HttpGet("inactive")]
        public async Task<IActionResult> GetInActiveCustomers()
        {
            try
            {
                List<CustomerDTO> cust = await _CustomerService.GetInActiveCustomers();
                return Ok(cust);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        #endregion


        #region GetCustomerByPhone

        /// <summary>
        /// Get the customer details by Phone
        /// </summary>
        /// <param name="phone"></param>
        /// <returns></returns>
        /// 
        [HttpGet("phone/{phone}")]

        public async Task<IActionResult> GetCustomerByPhone(string phone)
        {
            try
            {
                List<CustomerwithAddressDTO> cust = await _CustomerService.GetCustomerByPhone(phone);
                return Ok(cust);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


        #endregion


        #region UpdateFirstNameById
        /// <summary>
        /// Update the FirstName of a customer by CustomerId
        /// </summary>
        /// <param name="id"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        /// 

        [HttpPut("UpdateFirstNameById/{id}")]
        public async Task<IActionResult> UpdateFirstNameById(int id, string name)
        {
            try
            {
               
               var result = await _CustomerService.UpdateFirstNameById(id,name);
                return Ok(result);

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        #endregion


        #region UpdateLastNameById

        /// <summary>
        /// Update LastName of a Customer by CustomerId
        /// </summary>
        /// <param name="id"></param>
        /// <param name="lastname"></param>
        /// <returns></returns>

        [HttpPut("UpdateLastNameById/{id}")]
        public async Task<IActionResult> UpdateLastNameById(int id, string lastname)
        {
            try
            {

                var result = await _CustomerService.UpdateLastNameById(id,lastname);
                return Ok(result);

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        #endregion


        #region UpdateEmailCustomer

        /// <summary>
        /// Update Email of a Customer by CustomerId
        /// </summary>
        /// <param name="id"></param>
        /// <param name="email"></param>
        /// <returns></returns>
        
        [HttpPut("UpdateEmailByid/{id}")]
        public async Task<IActionResult> UpdateEmailCustomer(int id, string email)
        {
            try
            {

                var result = await _CustomerService.UpdateEmailCustomer(id, email);
                return Ok(result);

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        #endregion


        #region UpdatePhoneCustomer

        /// <summary>
        /// Update the Phone number details of Customer by CustomerId
        /// </summary>
        /// <param name="id"></param>
        /// <param name="phone"></param>
        /// <returns></returns>

        [HttpPut("UpdatePhoneNumberByid/{id}")]
        public async Task<IActionResult> UpdatePhoneCustomer(int id, string phone)
        {
            try
            {

               var result = await _CustomerService.UpdatePhoneCustomer(id, phone);
                return Ok(result);

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


        #endregion


        #region AssignStoreToCustomer

        /// <summary>
        /// Assigning a new Storeid to customer by CustomerId
        /// </summary>
        /// <param name="customerid"></param>
        /// <param name="storeid"></param>
        /// <returns></returns>

        [HttpPut("AssignStoreToCustomer")]
        public async Task<IActionResult> AssignStoreToCustomer(int customerid, int storeid)
        {
            try
            {
                if(customerid == 0 || storeid == 0)
                {
                    return BadRequest("Customerid or store is Required");
                }
                var result = await _CustomerService.AssignStoreToCustomer(customerid,storeid);
                return Ok(result);

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        #endregion













    }
}
