using System.ComponentModel;
using System.Net;
using AutoMapper;
using FilmRentalStore.DTO;
using FilmRentalStore.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;

namespace FilmRentalStore.Services
{
    public class StoreClass : IStoreRepository
    {
        private readonly Sakila12Context _context;
        public readonly IMapper _mapper;
        public StoreClass(Sakila12Context context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        #region Addstore
        /// <summary>
        /// Adding a store details to store table
        /// </summary>
        /// <param name="storeDto"></param>
        /// <returns></returns>
        public async Task AddStore(StoreDTO storeDto)
        {
            var s = _mapper.Map<Store>(storeDto);
            _context.Stores.Add(s);
            await _context.SaveChangesAsync();


        }
        #endregion
        #region GetStoreByCity
        /// <summary>
        /// fetching store details by city name
        /// </summary>
        /// <param name="city"></param>
        /// <returns></returns>
        public async Task<List<StoreDTO>> GetStoreByCity(string city)
        {
            var stores = await _context.Stores.FromSqlRaw("EXEC GetStoreByCity @CityName={0}", city).ToListAsync();
            /*var store = await (
                from stores in _context.Stores
                join address in _context.Addresses on stores.AddressId equals address.AddressId
                join cityEntity in _context.Cities on address.CityId equals cityEntity.CityId
                where cityEntity.City1.Equals(city)
                select new StoreDTO
                {
                    StoreId = stores.StoreId,
                    ManagerStaffId = stores.ManagerStaffId,
                    AddressId = stores.AddressId,



                })
                .ToListAsync();
            var value = _mapper.Map<List<StoreDTO>>(store);
            return value;*/
            var storage = _mapper.Map<List<StoreDTO>>(stores);
            return storage;
        }

        #endregion
        #region GetStoreByCountry
        /// <summary>
        /// fetching store details by country
        /// </summary>
        /// <param name="country"></param>
        /// <returns></returns>
        public async Task<List<StoreDTO>> GetStoreByCountry(string country)
        {
            var store = await (
                from stores in _context.Stores
                join address in _context.Addresses on stores.AddressId equals address.AddressId
                join cityEntity in _context.Cities on address.CityId equals cityEntity.CityId
                join countryEntity in _context.Countries on cityEntity.CountryId equals countryEntity.CountryId
                where countryEntity.Country1.Equals(country)
                select new StoreDTO
                {
                    StoreId = stores.StoreId,
                    ManagerStaffId = stores.ManagerStaffId,
                    AddressId = stores.AddressId,
                    LastUpdate=stores.LastUpdate.Date

                   
                })
                .ToListAsync();
            var value = _mapper.Map<List<StoreDTO>>(store);
            return value;
        }

        #endregion
        #region GetAllStaffDetailsbystore
        /// <summary>
        /// getting all staff details by store id
        /// </summary>
        /// <param name="storeid"></param>
        /// <returns></returns>
        public async Task<List<StaffDTO>> GetAllStaffOfStore(int storeid)
        {
            var storebystaff = await _context.Staff.Where(o => o.StoreId == storeid).ToListAsync();

            return _mapper.Map<List<StaffDTO>>(storebystaff);
         }
        #endregion
        #region GetAllCustomerdetails
        /// <summary>
        /// get all customer details by store id
        /// </summary>
        /// <param name="storeid"></param>
        /// <returns></returns>
        public async Task<List<CustomerDTO>> GetAllCustomers(int storeid)
        {
            var res = await _context.Customers.Where(o => o.StoreId == storeid).ToListAsync();
            return _mapper.Map<List<CustomerDTO>>(res);
        }
        #endregion
        #region AssignAddress
        /// <summary>
        /// assigning address to address table wth storeid
        /// </summary>
        /// <param name="storeid"></param>
        /// <param name="addressDto"></param>
        /// <returns></returns>
        public async Task AssignAddress(int storeid, AddressDTO addressDto)
        {

            var store = await _context.Stores.FindAsync(storeid);
            if(store == null)
            {
                throw new ArgumentException("No store found");
            }
            store.AddressId = addressDto.AddressId;
            var staff = await _context.Addresses.FindAsync(addressDto.AddressId);

            if(staff == null)
            {
                throw new ArgumentException("address not found");
            }
            _mapper.Map(addressDto, staff);
            /* staff.AddressId = addressDto.AddressId;
             staff.Address1 = addressDto.Address1;
             staff.Address2 = addressDto.Address2;
             staff.CityId = addressDto.CityId;
             staff.PostalCode = addressDto.PostalCode;
             staff.District = addressDto.District;*/

            _context.Stores.Update(store);
            _context.Addresses.Update(staff);

            await _context.SaveChangesAsync();

        }
        #endregion
        # region GetStorebyPhoneno
        /// <summary>
        /// get store details with help of phoneno in address
        /// </summary>
        /// <param name="phone"></param>
        /// <returns></returns>
        /// <exception cref="KeyNotFoundException"></exception>
        public async Task<StoreDTO> GetStorebyPhoneno(string phone)
        {
            var res = await _context.Stores.Include(s => s.Address).FirstOrDefaultAsync(s => s.Address.Phone == phone);
            if (res == null)
            {
                throw new KeyNotFoundException("Store not found");

            }
            return new StoreDTO
            {
                AddressId = res.AddressId,
                StoreId = res.StoreId,
                ManagerStaffId = res.ManagerStaffId,
                LastUpdate = DateTime.UtcNow,
            };
        }
        #endregion
        #region UpdatePhoneBystoreid
        /// <summary>
        /// update the phone num for particular storeid
        /// </summary>
        /// <param name="storeid"></param>
        /// <param name="phone"></param>
        /// <returns></returns>
        /// <exception cref="KeyNotFoundException"></exception>
        public async Task UpdatePhoneBystoreid(int storeid,string phone)
        {
            var res = await _context.Stores.Include(s => s.Address).FirstOrDefaultAsync(s => s.StoreId == storeid);
            if (res == null)
            {
                throw new KeyNotFoundException("store not found");
            }
            res.Address.Phone = phone;
            res.LastUpdate = DateTime.UtcNow;
            await _context.SaveChangesAsync();
        }
        #endregion
        #region AssignManager
        /// <summary>
        /// add manager details to staff table with store id
        /// </summary>
        /// <param name="storeid"></param>
        /// <param name="staffDto"></param>
        /// <returns></returns>
        public async Task AssignManager(int storeid, StaffDTO staffDto)
        {
            var store = await _context.Stores.FindAsync(storeid);
            if (store == null)
            {
                throw new ArgumentException("Storeid doesn't exist");
            }
            store.ManagerStaffId = staffDto.StaffId;
            var staff = await _context.Staff.FindAsync(staffDto.StaffId);

            if (staff == null)
            {
                throw new ArgumentException("address not found");
            }

<<<<<<< HEAD
            staff.AddressId = staffDto.AddressId;
            staff.FirstName = staffDto.FirstName;
            staff.LastName = staffDto.LastName;
            staff.Email = staffDto.Email;
            staff.Active = staffDto.Active;
            staff.UrlPath = staffDto.UrlPath;
            staff.StaffId = staffDto.StaffId;
=======
            
            _mapper.Map(staffDto, staff);
>>>>>>> origin/FilmRentalStore-4
            

            _context.Stores.Update(store);
            _context.Staff.Update(staff);

            await _context.SaveChangesAsync();


        }
        #endregion
        #region GetAllManagerofstore
        /// <summary>
        /// getting all manager details by store id
        /// </summary>
        /// <param name="storeid"></param>
        /// <returns></returns>
        public async Task<List<StaffDTO>> GetAllManagerOfStore(int storeid)
        {
            var storebystaff = await _context.Staff.Where(o => o.StoreId == storeid).ToListAsync();

            return _mapper.Map<List<StaffDTO>>(storebystaff);

        }
        #endregion
        #region GetAllStaffandstoredetails
        /// <summary>
        /// get all staff and store details
        /// </summary>
        /// <returns></returns>
        public async Task<List<JoinDTO>> GetAllStaffAndStore()
        {
            var staffandstore = await (from s in _context.Stores
                                       join m in _context.Staff
                                       on s.StoreId equals m.StoreId
                                       join a in _context.Addresses on s.AddressId equals a.AddressId
                                       join c in _context.Cities on a.CityId equals c.CityId
                                       select new JoinDTO
                                       {
                                           FirstName = m.FirstName,
                                           LastName = m.LastName,
                                           Email = m.Email,
                                           Phone = a.Phone,
                                           Address1 = a.Address1,
                                           Address2 = a.Address2,
                                           City1 = c.City1,

                                       }).ToListAsync();
            return staffandstore;
        }
        #endregion

    }
}