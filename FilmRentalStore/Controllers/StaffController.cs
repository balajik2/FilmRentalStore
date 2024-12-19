using FilmRentalStore.DTO;
using FilmRentalStore.Model;
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
                return BadRequest(ex.Message);
            }
        }

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
                return Ok("Phone number updated successfully");

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
