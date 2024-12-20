using FilmRentalStore.DTO;
using FilmRentalStore.Models;
using FilmRentalStore.Services;
using FluentValidation;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace FilmRentalStore.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StaffController : ControllerBase
    {
        private readonly IStaffRepository _context;
        private readonly IConfiguration _configuration;

        //Fluent Validation
        private readonly IValidator<StaffDTO> _validator;

        public StaffController(IStaffRepository staff, IConfiguration configuration, IValidator<StaffDTO> validator)
        {
            _context = staff;
            _configuration = configuration;
            _validator = validator;
            
        }

        #region GetStaff

        /// <summary> Get Staff
        /// 
        /// It retrieves staff data and handles both successful and error responses.
        /// 
        /// </summary>
        /// <returns></returns>

        [HttpGet("GetStaff")]
        public async Task<IActionResult> GetStaff()
        {
            try
            {
                var staff = await _context.GetStaff();
                return Ok(staff);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        #endregion


        #region AddStaff

        /// <summary> Add Staff
        /// 
        /// The AddStaff method handles a POST request to add new staff. 
        /// It first checks if the input data (staffDTO) is valid.
        /// If it's not valid, it returns an error with the validation issues.
        /// If the data is valid, it tries to add the staff to the system. 
        /// 
        /// </summary>
        /// <param name="staffDTO"></param>
        /// <returns></returns>

        [HttpPost("AddStaff")]
        public async Task<IActionResult> AddStaff(StaffDTO staffDTO)
        {
            var validationResult = _validator.Validate(staffDTO);
            try
            {
                if (!validationResult.IsValid)
                {
                    return BadRequest(validationResult.Errors);
                }
                await _context.AddStaff(staffDTO);
               
                return CreatedAtAction("GetStaff", new { }, staffDTO);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
                if (ex.InnerException != null)
                {
                    Console.WriteLine("Inner Exception: " + ex.InnerException.Message);
                }
                return BadRequest(ex.Message);
            }
        }

        #endregion


        #region GetStaffByLastName

        /// <summary>  GetStaffByLastName
        /// 
        /// This method handles a GET request to retrieve staff based on their last name.
        /// It first checks if the last name is provided; if not, it returns an error message saying "Last name is required.
        /// " If a valid last name is given, it fetches the corresponding staff details and returns them.
        /// 
        /// </summary>
        /// <param name="lastname"></param>
        /// <returns></returns>

        [HttpGet("GetStaffByLastName")]
        public async Task<IActionResult> GetStaffByLastName(string lastname)
        {
            try
            {
                if (string.IsNullOrEmpty(lastname))
                {
                    return BadRequest("Last name is required");
                }
                var staff = await _context.GetStaffByLastName(lastname);
                return Ok(staff);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        #endregion


        #region GetStaffByFirstName

        /// <summary> GetStaffByFirstName
        /// 
        /// This method handles a GET request to retrieve staff based on their first name.
        /// It first checks if the first name is provided; if not, it returns an error message saying "First name is required.
        /// " If a valid first name is given, it fetches the matching staff details and returns them in the response.
        /// 
        /// </summary>
        /// <param name="firstname"></param>
        /// <returns></returns>

        [HttpGet("GetStaffByFirstName")]
        public async Task<IActionResult> GetStaffByFirstName(string firstname)
        {
            try
            {
                if (string.IsNullOrEmpty(firstname))
                {
                    return BadRequest("First name is required");
                }
                List<StaffDTO> staff = await _context.GetStaffByFirstName(firstname);
                return Ok(staff);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        #endregion


        #region GetStaffByEmail

        /// <summary> GetStaffByEmail
        /// 
        /// This method handles a GET request to retrieve staff based on their email address.
        /// It first checks if the email is provided; if not, it returns an error message saying "Email is required.
        /// " If a valid email is given, it fetches the matching staff details and returns them in the response. 
        /// 
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>

        [HttpGet("GetStaffByEmail")]
        public async Task<IActionResult> GetStaffByEmail(string email)
        {
            try
            {
                if (string.IsNullOrEmpty(email))
                {
                    return BadRequest("Email is required");
                }
                List<StaffDTO> staff = await _context.GetStaffByEmail(email);
                return Ok(staff);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        #endregion


        #region AssignAddress

        /// <summary> AssignAddress
        /// 
        /// This method handles the assignment of an address to a staff member.
        /// It first checks if the staffDTO is provided and whether both StaffId and AddressId are valid (non-zero).
        /// If any of these conditions are not met, it returns an error message saying "StaffId and AddressId are required.
        /// " If the validation passes, it calls the AssignAddress method to assign the address to the staff member and returns the result. 
        ///  
        /// </summary>
        /// <param name="staffDTO"></param>
        /// <returns></returns>


        [HttpPut("AssignAddress")]
        public async Task<IActionResult> AssignAddress(StaffDTO staffDTO)
        {
            try
            {
                if (staffDTO == null || staffDTO.StaffId == 0 || staffDTO.AddressId == 0)
                {
                    return BadRequest("StaffId and AddressId are required.");
                }

                var result = await _context.AssignAddress(staffDTO);
                return Ok(result);

            }
            catch (Exception ex)
            { 
                return BadRequest(ex.Message);
            }
        }

        #endregion


        #region GetStaffByCity

        /// <summary> GetStaffByCity
        /// 
        /// This method handles a GET request to retrieve staff based on their city.
        /// It first checks if the city is provided; if not, it returns an error message saying "City is required.
        /// " If a valid city is given, it fetches the matching staff details and returns them in the response.
        /// 
        /// </summary>
        /// <param name="city"></param>
        /// <returns></returns>


        [HttpGet("GetStaffByCity")]
        public async Task<IActionResult> GetStaffByCity(string city)
        {
            try
            {
                if (string.IsNullOrEmpty(city))
                {
                    return BadRequest("City is required");
                }
                List<StaffDTO> staff = await _context.GetStaffByCity(city);
                return Ok(staff);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        #endregion


        #region GetStaffByCountry

        /// <summary>  GetStaffByCountry
        /// 
        /// This method handles a GET request to retrieve staff based on their country.
        /// It first checks if the country is provided; if not, it returns an error message saying "Country is required.
        /// " If a valid country is given, it fetches the matching staff details and returns them in the response.
        /// 
        /// </summary>
        /// <param name="country"></param>
        /// <returns></returns>


        [HttpGet("GetStaffByCountry")]
        public async Task<IActionResult> GetStaffByCountry(string country)
        {
            try
            {
                if (string.IsNullOrEmpty(country))
                {
                    return BadRequest("City is required");
                }
                List<StaffDTO> staff = await _context.GetStaffByCountry(country);
                return Ok(staff);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        #endregion


        #region GetStaffByPhoneNumber

        /// <summary> GetStaffByPhoneNumber
        /// 
        /// This method handles a GET request to retrieve staff based on their phone number.
        /// It first checks if the phone number is provided; if not, it returns an error message saying "Phone number is required.
        /// " If a valid phone number is given, it fetches the matching staff details and returns them in the response.
        /// 
        /// </summary>
        /// <param name="phone"></param>
        /// <returns></returns>

        [HttpGet("GetStaffByPhoneNumber")]
        public async Task<IActionResult> GetStaffByPhoneNumber(string phone)
        {
            try
            {
                if (string.IsNullOrEmpty(phone))
                {
                    return BadRequest("City is required");
                }
                List<StaffDTO> staff = await _context.GetStaffByPhoneNumber(phone);
                return Ok(staff);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        #endregion


        #region UpdateStaffByFirstName

        /// <summary> UpdateStaffByFirstName
        /// 
        /// This method handles a PUT request to update a staff member's first name based on their staff ID.
        /// It first checks if the new first name is provided; if not, it returns an error message saying "First name is required.
        /// " If a valid first name is given, it calls the UpdateStaffByFirstName method to update the staff's first name and returns the result. 
        /// 
        /// </summary>
        /// <param name="staffId"></param>
        /// <param name="newfirstname"></param>
        /// <returns></returns>

        [HttpPut("UpdateStaffByFirstName")]
        public async Task<IActionResult> UpdateStaffByFirstName(int staffId, string newfirstname)
        {
            try
            {
                if (string.IsNullOrEmpty(newfirstname))
                {
                    return BadRequest("StaffId are required.");
                }

                var result = await _context.UpdateStaffByFirstName(staffId, newfirstname);
                return Ok(result);

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        #endregion


        #region UpdateStaffByLastName

        /// <summary>  UpdateStaffByLastName
        /// 
        /// This method handles a PUT request to update a staff member's last name based on their staff ID.
        /// It first checks if the new last name is provided; if not, it returns an error message saying "Last name is required.
        /// " If a valid last name is given, it calls the UpdateStaffByLastName method to update the staff's last name and returns the result.
        /// 
        /// </summary>
        /// <param name="staffId"></param>
        /// <param name="newlastname"></param>
        /// <returns></returns>


        [HttpPut("UpdateStaffByLastName")]
        public async Task<IActionResult> UpdateStaffByLastName(int staffId, string newlastname)
        {
            try
            {
                if (string.IsNullOrEmpty(newlastname))
                {
                    return BadRequest("StaffId are required.");
                }

                var result = await _context.UpdateStaffByLastName(staffId, newlastname);
                return Ok(result);

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        #endregion


        #region UpdateStaffByEmail

        /// <summary> UpdateStaffByEmail
        /// 
        /// This method handles a PUT request to update a staff member's email based on their staff ID.
        /// It first checks if the email is provided; if not, it returns an error message saying "Email is required.
        /// " If a valid email is given, it calls the UpdateStaffByEmail method to update the staff's email and returns the result.
        /// 
        /// </summary>
        /// <param name="staffId"></param>
        /// <param name="email"></param>
        /// <returns></returns>

        [HttpPut("UpdateStaffByEmail")]
        public async Task<IActionResult> UpdateStaffByEmail(int staffId, string email)
        {
            try
            {
                if (string.IsNullOrEmpty(email))
                {
                    return BadRequest("StaffId are required.");
                }

                var result = await _context.UpdateStaffByEmail(staffId, email);
                return Ok(result);

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        #endregion


        #region AssignStoreToStaff

        /// <summary> AssignStoreToStaff
        /// 
        /// This method handles a PUT request to assign a store to a staff member based on their staff ID and store ID.
        /// It first checks if both the staffId and storeId are provided and valid; if either is missing or zero, it returns an error message saying "StaffId and StoreId are required.
        /// " If valid IDs are provided, it calls the AssignStoreToStaff method to assign the store to the staff member and returns the result.
        /// 
        /// </summary>
        /// <param name="staffId"></param>
        /// <param name="storeId"></param>
        /// <returns></returns>

        [HttpPut("AssignStoreToStaff")]
        public async Task<IActionResult> AssignStoreToStaff(int staffId,int storeId)
        {
            try
            {
                if (staffId == 0 || storeId == 0)
                {
                    return BadRequest("StaffId and AddressId are required.");
                }

                var result = await _context.AssignStoreToStaff(staffId, storeId);
                return Ok(result);

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        #endregion


        #region UpdatePhoneNumberByStaff

        /// <summary> UpdatePhoneNumberByStaff
        /// 
        /// This method handles a PUT request to update a staff member's phone number based on their staff ID.
        /// It first checks if the new phone number is provided; if not, it returns an error message saying "Phone number is required.
        /// " If a valid phone number is given, it calls the UpdatePhoneNumberByStaff method to update the staff's phone number and returns a success response with the updated data and a message: "Phone number updated successfully." 
        /// 
        /// </summary>
        /// <param name="staffId"></param>
        /// <param name="newPhone"></param>
        /// <returns></returns>

        [HttpPut("UpdatePhoneNumberByStaff")]
        public async Task<IActionResult> UpdatePhoneNumberByStaff(int staffId, string newPhone)
        {
            try
            {
                if (string.IsNullOrEmpty(newPhone))
                {
                    return BadRequest("Phone number is required.");
                }

                var result = await _context.UpdatePhoneNumberByStaff(staffId, newPhone);
                return Ok(new { data = result, message = "Phone number updated successfully" });

                //return Ok("Phone number updated successfully");

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        #endregion
    
    }
}
