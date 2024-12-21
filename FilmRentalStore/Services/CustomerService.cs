using AutoMapper;
using FilmRentalStore.DTO;
using FilmRentalStore.Models;
using Microsoft.EntityFrameworkCore;

namespace FilmRentalStore.Services
{
    public class CustomerService : ICustomerRepository
    {
        private readonly Sakila12Context _context;

        private readonly IMapper _mapper;

        public CustomerService(Sakila12Context context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        #region AddCustomer
        public async Task AddCustomer(CustomerDTO customer)
        {
            var customerdto = _mapper.Map<Customer>(customer);
            _context.Customers.Add(customerdto);
            await _context.SaveChangesAsync();


        }
        #endregion

        #region GetCustomer
        public async Task<List<CustomerDTO>> GetCustomer()
        {
            var List = await _context.Customers.ToListAsync();
            var DTOList = _mapper.Map<List<CustomerDTO>>(List);
            return DTOList;
        }
        #endregion


        #region GetCustomerByLastName

        public async Task<List<CustomerDTO>> GetCustomerByLastName(string lastname)
        {
            var s = await _context.Customers.Where(x => x.LastName == lastname).ToListAsync();
            
            var value = _mapper.Map<List<CustomerDTO>>(s);
            return value;
        }

        #endregion


        #region GetCustomerByFirstName

        public async Task<List<CustomerDTO>> GetCustomerByFirstName(string firstname)
        {
            var s = await _context.Customers.Where(x => x.FirstName == firstname).ToListAsync();

            var value = _mapper.Map<List<CustomerDTO>>(s);
            return value;
        }

        #endregion


        #region GetCustomerByEmail

       public async Task<CustomerDTO> GetCustomerByEmail(string? email)
        {
            var s = await _context.Customers.FirstOrDefaultAsync(x => x.Email == email);

            var value = _mapper.Map<CustomerDTO>(s);
            return value;
        }

        #endregion



  
        #region AssignAddress
        public async Task<List<CustomerDTO>> AssignAddress(CustomerDTO customer)
        {
            var cust = await _context.Customers.FirstOrDefaultAsync(s => s.CustomerId == customer.CustomerId);

            if (cust == null)
            {
                return null;
            }

            var address = await _context.Addresses.FirstOrDefaultAsync(s => s.AddressId == customer.AddressId);

            if (address == null)
            {
                return null;
            }

            cust.AddressId = customer.AddressId;
            await _context.SaveChangesAsync();

            //To fetch the updated details list
            var updatedlist = await _context.Customers.Where(s => s.CustomerId == cust.CustomerId).ToListAsync();

            //map and return the details
            return _mapper.Map<List<CustomerDTO>>(updatedlist);
        }
        #endregion

        #region GetCustomerByCity
        public async Task<List<CustomerwithAddressDTO>> GetCustomerByCity(string city)
        {
            var customers = await (
                from customer in _context.Customers
                join address in _context.Addresses on customer.AddressId equals address.AddressId
                join cityEntity in _context.Cities on address.CityId equals cityEntity.CityId
                where cityEntity.City1 == city // Filter by city name
                select new CustomerwithAddressDTO
                {
                    CustomerId = customer.CustomerId,
                    FirstName = customer.FirstName,
                    LastName = customer.LastName,
                    Email = customer.Email,
                   
                        AddressId = address.AddressId,
                        Address1 = address.Address1,
                        Address2 = address.Address2,
                        District = address.District,
                        CityId = address.CityId,
                        PostalCode = address.PostalCode,
                        Phone = address.Phone,
                        LastUpdate = address.LastUpdate
                    
                })
                .ToListAsync();
            var value = _mapper.Map<List<CustomerwithAddressDTO>>(customers);
            return value;
        }

        #endregion

        #region GetCustomerByCountry
        public async Task<List<CustomerDTO>> GetCustomerByCountry(string country)
        {
            var customers = await (
               from customer in _context.Customers
               join address in _context.Addresses on customer.AddressId equals address.AddressId
               join cityEntity in _context.Cities on address.CityId equals cityEntity.CityId
               join countryEntity in _context.Countries on cityEntity.CountryId equals countryEntity.CountryId
               where countryEntity.Country1 == country
               select new CustomerDTO
               {
                   CustomerId = customer.CustomerId,
                   FirstName = customer.FirstName,
                   LastName = customer.LastName,
                   Email = customer.Email,

               }).ToListAsync();


            var value = _mapper.Map<List<CustomerDTO>>(customers);
            return value;
            
        }

        #endregion

        #region GetActiveCustomers
        public async Task<List<CustomerDTO>> GetActiveCustomers()
        {
            var s = await _context.Customers.Where(x => x.Active == "1").ToListAsync();

            var value = _mapper.Map<List<CustomerDTO>>(s);
            return value;
        }
        #endregion


        #region GetInActiveCustomers
        public async Task<List<CustomerDTO>> GetInActiveCustomers()
        {
            var inactive = await _context.Customers.Where(x => x.Active == "0").ToListAsync();

            var value = _mapper.Map<List<CustomerDTO>>(inactive);
            return value;
        }
        #endregion
       



        #region GetCustomerByPhone
        public async Task<List<CustomerwithAddressDTO>> GetCustomerByPhone(string phone)
        {
            var customerdetails = await (
                from customer in _context.Customers
                join address in _context.Addresses on customer.AddressId equals address.AddressId
                where address.Phone == phone
                select new CustomerwithAddressDTO
                {
                    CustomerId = customer.CustomerId,
                    StoreId = customer.StoreId,
                    FirstName = customer.FirstName,
                    LastName = customer.LastName,
                    Email = customer.Email,
                    AddressId = address.AddressId,
                    Active = customer.Active,
                    CreateDate = customer.CreateDate,
                    LastUpdate = customer.LastUpdate,
                    Address1 = address.Address1,
                    Address2 = address.Address2,
                    District = address.District,
                    CityId = address.CityId,
                    PostalCode = address.PostalCode,
                    Phone = address.Phone
                })
                .ToListAsync();
            return customerdetails;
        }
        #endregion


        #region UpdateFirstNameById
        public async Task<List<CustomerDTO>> UpdateFirstNameById(int id, string name)
        {
            var existingUser = await _context.Customers.FindAsync(id);
           
            if (existingUser == null)
            {
                throw new KeyNotFoundException($"User with ID {id} not found");
            }

            existingUser.FirstName = name;

            await _context.SaveChangesAsync();

            var updatedList = await _context.Customers.Where(s=>s.CustomerId == existingUser.CustomerId).ToListAsync();
            var result = _mapper.Map<List<CustomerDTO>>(updatedList);
            return result;

           
        }
        #endregion


        #region UpdateLastNameById
        public async Task<List<CustomerDTO>> UpdateLastNameById(int id, string lastname)
        {
            var existingUser = await _context.Customers.FindAsync(id);

            if (existingUser == null)
            {
                throw new KeyNotFoundException($"User with ID {id} not found");
            }

            existingUser.LastName = lastname;

            await _context.SaveChangesAsync();

            var updatedList = await _context.Customers.Where(s => s.CustomerId == existingUser.CustomerId).ToListAsync();
            var result = _mapper.Map<List<CustomerDTO>>(updatedList);
            return result;
        }


        #endregion


        #region UpdateEmailCustomer
        public async Task<List<CustomerDTO>> UpdateEmailCustomer(int id, string email)
        {
            var existingUser = await _context.Customers.FindAsync(id);

            if (existingUser == null)
            {
                throw new KeyNotFoundException($"User with ID {id} not found");
            }

            existingUser.Email = email;

            await _context.SaveChangesAsync();

            var updatedList = await _context.Customers.Where(s => s.CustomerId == existingUser.CustomerId).ToListAsync();
            var result = _mapper.Map<List<CustomerDTO>>(updatedList);
            return result;
        }
        #endregion


        #region UpdatePhoneCustomer
        public async Task<List<CustomerDTO>> UpdatePhoneCustomer(int id, string phone)
        {
            
            var customer = await _context.Customers
                .FirstOrDefaultAsync(c => c.CustomerId == id);

            if (customer == null)
            {
                throw new KeyNotFoundException($"Customer with ID {id} not found.");
            }

            
            var address = await _context.Addresses
                .FirstOrDefaultAsync(a => a.AddressId == customer.AddressId);

            if (address == null)
            {
                throw new KeyNotFoundException($"Address for Customer ID {id} not found.");
            }

           
            address.Phone = phone;

        
            await _context.SaveChangesAsync();

            var updatedvalue =await _context.Customers.Where(s=>s.CustomerId== customer.CustomerId).Include(s=>s.Address).ToListAsync();
            return _mapper.Map<List<CustomerDTO>>(updatedvalue);
        }

        #endregion


        #region AssignStoreToCustomer
        public async Task<List<CustomerDTO>> AssignStoreToCustomer(int customerid, int storeid)
        {
            var cust = await _context.Customers.FirstOrDefaultAsync(s => s.CustomerId == customerid);
            if (cust == null)
            {
                return null;
            }

            // Assign the store to the staff member (assuming Staff has a Store property)
            cust.StoreId = storeid;

           

         //   _context.Staff.Update(staff);
            await _context.SaveChangesAsync();

            var updatedlist = await _context.Customers.Where(s => s.CustomerId == cust.CustomerId).ToListAsync();


            return _mapper.Map<List<CustomerDTO>>(updatedlist);
        }

        #endregion



    }
}
