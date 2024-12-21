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

                await _CustomerService.AddCustomer(customer);
                 return CreatedAtAction("GetCustomer", new { }, customer);
                //return Ok(customervalue);
            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        #endregion


        #region GetCustomer
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

        #region GetCustomerByLastName
        [HttpGet("lastname/{lastname}")]

        public async Task<IActionResult> GetCustomerByLastName(string lastname)
        {
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
        [HttpGet("firstname/{firstname}")]

        public async Task<IActionResult> GetCustomerByFirstName(string firstname)
        {
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

        [HttpGet("email/{email}")]
        public async Task<IActionResult> GetCustomerByEmail(string? email)
        {
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

        [HttpPut("AsignAddress")]
        public async Task<IActionResult> AssignAddress(CustomerDTO customer)
        {
            try
            {
                if (customer == null || customer.CustomerId == 0 || customer.CustomerId == 0)
                {
                    return BadRequest("CustomerId and AddressId are required.");
                }

                var result = await _CustomerService.AssignAddress(customer);
                return Ok(result);

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        #endregion



        #region GetCustomerByCity
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

        [HttpGet("country/{country}")]


       
        public async Task<IActionResult> GetCustomerByCountry(string country)
        {
            try
            {
                List<CustomerDTO> cust = await _CustomerService.GetCustomerByCountry(country);
                return Ok(cust);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        #endregion


        #region GetActiveCustomers

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

        [HttpPut("UpdatePhoneNumberByid/{id}")]
        public async Task<IActionResult> UpdatePhoneCustomer(int id, string phone)
        {
            try
            {

               var result = await _CustomerService.UpdatePhoneCustomer(id, phone);
                return Ok(new { data = result, Message = "Phone number updated Successfully" });

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


        #endregion


        #region AssignStoreToCustomer

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
