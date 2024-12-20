using AutoMapper;
using FilmRentalStore.DTO;
using FilmRentalStore.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics.Metrics;
using System.Numerics;

namespace FilmRentalStore.Services
{
    public class StaffService : IStaffRepository
    {
        public readonly Sakila12Context _context;
        public readonly IMapper _mapper;

        public StaffService(Sakila12Context sakila, IMapper mapper)
        {
            _context = sakila;
            _mapper = mapper;
        }

        #region GetStaff
        public async Task<List<StaffDTO>> GetStaff()
        {
            var List = await _context.Staff.ToListAsync();
            var DTOList = _mapper.Map<List<StaffDTO>>(List);
            return DTOList;
        }

        #endregion

        #region AddStaff

        public async Task AddStaff(StaffDTO staffDTO)
        {
            var Staff = _mapper.Map<Staff>(staffDTO);
            _context.Staff.Add(Staff);
            await _context.SaveChangesAsync();
        }

        #endregion

        #region GetStaffByLastName

        public async Task<List<StaffDTO>> GetStaffByLastName(string lastname)
        {
            //EF.Functions.Like(s.LastName, $"{lastname}%")
            var staffList = await _context.Staff.Where(s => s.LastName == lastname).ToListAsync();

            var staffDTOList = _mapper.Map<List<StaffDTO>>(staffList);
            return staffDTOList;
        }

        #endregion

        #region GetStaffByFirstName
        public async Task<List<StaffDTO>> GetStaffByFirstName(string firstname)
        {
            var staffList = await _context.Staff.Where(s => s.FirstName == firstname).ToListAsync();

            var staffDTOList = _mapper.Map<List<StaffDTO>>(staffList);
            return staffDTOList;
        }

        #endregion

        #region GetStaffByEmail
        public async Task<List<StaffDTO>> GetStaffByEmail(string email)
        {
            var staffList = await _context.Staff.Where(s => s.Email == email).ToListAsync();

            var staffDTOList = _mapper.Map<List<StaffDTO>>(staffList);
            return staffDTOList;
        }

        #endregion

        #region AssignAddress

        public async Task<List<StaffDTO>> AssignAddress(StaffDTO staffDTO)
        {
            var staff = await _context.Staff.FirstOrDefaultAsync(s => s.StaffId == staffDTO.StaffId);

            if (staff == null)
            {
                return null;
            }

            var address = await _context.Addresses.FirstOrDefaultAsync(s => s.AddressId == staffDTO.AddressId);

            if (address == null)
            {
                return null;
            }

            staff.AddressId = staffDTO.AddressId;
            await _context.SaveChangesAsync();

            //To fetch the updated details list
            var updatedlist = await _context.Staff.Where(s => s.StaffId == staff.StaffId).ToListAsync();

            //map and return the details
            return _mapper.Map<List<StaffDTO>>(updatedlist);
        }

        #endregion

        #region GetStaffByCity
        public async Task<List<StaffDTO>> GetStaffByCity(string city)
        {
            var stafflist = await (from staff in _context.Staff
                                   join address in _context.Addresses on staff.AddressId equals address.AddressId
                                   join cityEntity in _context.Cities on address.CityId equals cityEntity.CityId
                                   where cityEntity.City1 == city

                                   select new StaffDTO
                                   {
                                       StaffId = staff.StaffId,
                                       FirstName = staff.FirstName,
                                       LastName = staff.LastName,
                                       Email = staff.Email,
                                       AddressId = staff.AddressId

                                   }).ToListAsync();

            return _mapper.Map<List<StaffDTO>>(stafflist);

        }

        #endregion

        # region GetStaffByCountry
        public async Task<List<StaffDTO>> GetStaffByCountry(string country)
        {
            var stafflist = await (from staff in _context.Staff
                                   join address in _context.Addresses on staff.AddressId equals address.AddressId
                                   join cityEntity in _context.Cities on address.CityId equals cityEntity.CityId
                                   join countryEntity in _context.Countries on cityEntity.CountryId equals countryEntity.CountryId
                                   where countryEntity.Country1 == country

                                   select new StaffDTO
                                   {
                                       StaffId = staff.StaffId,
                                       FirstName = staff.FirstName,
                                       LastName = staff.LastName,
                                       Email = staff.Email,
                                       AddressId = staff.AddressId

                                   }).ToListAsync();

            return _mapper.Map<List<StaffDTO>>(stafflist);
        }

        #endregion

        #region GetStaffByPhoneNumber

        public async Task<List<StaffDTO>> GetStaffByPhoneNumber(string phone)
        {
            var stafflist = await (from staff in _context.Staff
                                   join address in _context.Addresses on staff.AddressId equals address.AddressId
                                   where address.Phone == phone

                                   select new StaffDTO
                                   {
                                       StaffId = staff.StaffId,
                                       FirstName = staff.FirstName,
                                       LastName = staff.LastName,
                                       Email = staff.Email,
                                       AddressId = staff.AddressId

                                   }).ToListAsync();

            return _mapper.Map<List<StaffDTO>>(stafflist);
        }

        #endregion

        #region UpdateStaffByFirstName

        public async Task<List<StaffDTO>> UpdateStaffByFirstName(int staffId, string newfirstname)
        {
            var staff = await _context.Staff.FirstOrDefaultAsync(s => s.StaffId == staffId);

            if (staff == null)
            {
                return null;
            }


            staff.FirstName = newfirstname;
            await _context.SaveChangesAsync();

            //To fetch the updated details list
            var updatedlist = await _context.Staff.Where(s => s.StaffId == staff.StaffId).ToListAsync();

            //map and return the details
            return _mapper.Map<List<StaffDTO>>(updatedlist);
        }

        #endregion

        #region UpdateStaffByLastName

        public async Task<List<StaffDTO>> UpdateStaffByLastName(int staffId, string newlastname)
        {
            var staff = await _context.Staff.FirstOrDefaultAsync(s => s.StaffId == staffId);

            if (staff == null)
            {
                return null;
            }

            staff.LastName = newlastname;
            await _context.SaveChangesAsync();

            //To fetch the updated details list
            var updatedlist = await _context.Staff.Where(s => s.StaffId == staff.StaffId).ToListAsync();

            //map and return the details
            return _mapper.Map<List<StaffDTO>>(updatedlist);
        }

        #endregion

        #region UpdateStaffByEmail

        public async Task<List<StaffDTO>> UpdateStaffByEmail(int staffId, string email)
        {
            var staff = await _context.Staff.FirstOrDefaultAsync(s => s.StaffId == staffId);

            if (staff == null)
            {
                return null;
            }

            staff.Email = email;
            await _context.SaveChangesAsync();

            //To fetch the updated details list
            var updatedlist = await _context.Staff.Where(s => s.StaffId == staff.StaffId).ToListAsync();

            //map and return the details
            return _mapper.Map<List<StaffDTO>>(updatedlist);
        }

        #endregion

        #region AssignStoreToStaff

        public async Task<List<StaffDTO>> AssignStoreToStaff(int staffId, int storeId)
        {
            var store = await _context.Stores.FirstOrDefaultAsync(s => s.StoreId == storeId);
            if (store == null)
            {
                return null;
            }

            var staff = await _context.Staff.FirstOrDefaultAsync(s => s.StaffId == staffId);
            if (staff == null)
            {
                return null;
            }

            // Assign the store to the staff member (assuming Staff has a Store property)
            staff.StoreId = storeId;

            var updatedlist = await _context.Staff.Where(s => s.StaffId == staff.StaffId).ToListAsync();


            _context.Staff.Update(staff);
            await _context.SaveChangesAsync();

            return _mapper.Map<List<StaffDTO>>(updatedlist);
        }

        #endregion

        #region UpdatePhoneNumberByStaff

        public async Task<List<StaffDTO>> UpdatePhoneNumberByStaff(int staffId, string newPhone)
        {
            var staff = await _context.Staff.FirstOrDefaultAsync(s => s.StaffId == staffId);

            if (staff == null)
            {
                return null;
            }

            var address = await _context.Addresses.FirstOrDefaultAsync(s => s.AddressId == staff.AddressId);

            if (address == null)
            {
                return null;
            }

            address.Phone = newPhone;
            await _context.SaveChangesAsync();

            //To fetch the updated details list
            var updatedlist = await _context.Staff.Where(s => s.StaffId == staff.StaffId).Include(s => s.Address).ToListAsync();

            return _mapper.Map<List<StaffDTO>>(updatedlist);
        }

        #endregion
    }
}
