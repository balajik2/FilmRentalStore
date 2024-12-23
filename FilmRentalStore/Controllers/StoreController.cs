using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using AutoMapper;
using FilmRentalStore.DTO;
using FilmRentalStore.Models;
using Microsoft.EntityFrameworkCore;
using FluentValidation;
using FilmRentalStore.Services;
using System.Diagnostics.Metrics;
using System.ComponentModel;
namespace FilmRentalStore.Controllers
{
    [Route("api/[controller]")]
    [ApiController]

    public class StoreController : ControllerBase
    {

        private readonly Sakila12Context _context;
        private readonly IStoreRepository _StoreRepository;
        private readonly IValidator<StoreDTO> _validator;
        private readonly IValidator<AddressDTO> _validator2;
        
        public StoreController(IStoreRepository storeRepository, IValidator<StoreDTO> validator)
        {
            _StoreRepository = storeRepository;
            _validator = validator;
        }
        [HttpPost("post")]
        public async Task<IActionResult> Addstore(StoreDTO storeDto)
        {
            var validatorResult = _validator.Validate(storeDto);
            if (!validatorResult.IsValid)
            {
                return BadRequest(validatorResult.Errors);
            }
            await _StoreRepository.AddStore(storeDto);

            return Ok("Record created successfully");

        }
        [HttpGet("GetByCity")]
        public async Task<IActionResult> GetStoreByCity(string city)
        {
            try
            {
                List<StoreDTO> Findcity = await _StoreRepository.GetStoreByCity(city);

                return Ok(Findcity);


            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpGet("GetByCountry")]
        public async Task<IActionResult> GetStoreByCountry(string country)
        {
            try
            {
                List<StoreDTO> Findcountry = await _StoreRepository.GetStoreByCountry(country);

                return Ok(Findcountry);


            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("store / staff ")]
        public async Task<IActionResult> GetAllStaffOfStore(int storeid)
        {
            try
            {
                List<StaffDTO> staffdata = await _StoreRepository.GetAllStaffOfStore(storeid);

                return Ok(staffdata);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPut("address")]
        public async Task<IActionResult>  AssignAddress(int storeid, AddressDTO addressDto)
        {
            try
            {
                
                await _StoreRepository.AssignAddress(storeid,addressDto);
                return Ok("store & address details");
                
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpGet("All customers")]
        public async Task<IActionResult> GetAllCustomers(int storeid)
        {
            try
            {
                var customer = await _StoreRepository.GetAllCustomers(storeid);
                return Ok(customer);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpGet("/phone/{phone}")]
        public async Task<IActionResult> GetStorebyPhoneno(string phone)
        {
            try
            {
                var res = await _StoreRepository.GetStorebyPhoneno(phone);
                return Ok(res);
            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("updatephone")]
        public async Task<IActionResult> UpdatePhoneBystoreid(int storeid, string phone)
        {
            try
            {
                
                await _StoreRepository.UpdatePhoneBystoreid(storeid, phone);
                return Ok("Phone number updated successfully");
            }
            catch (KeyNotFoundException ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPut("updatemanagerdata")]

        public async Task<IActionResult> AssignManager(int storeid, StaffDTO staffDto)
        {
            try
            {
               
                await _StoreRepository.AssignManager(storeid, staffDto);
                return Ok("assigned successfully");
            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
                           

        }
        [HttpGet("managerdetails")]
        public async Task<IActionResult> GetAllManagerOfStore(int storeid)
        {
            try
            {
                var staffdata = await _StoreRepository.GetAllStaffOfStore(storeid);

                return Ok(staffdata);

            }

            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpGet("/api/store/managers")]
        public async Task<IActionResult> GetAllStaffAndStore()
        {
            try
            {
                var staffdata = await _StoreRepository.GetAllStaffAndStore();

                return Ok(staffdata);

            }

            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
    }

    

