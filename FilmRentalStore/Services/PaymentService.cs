using AutoMapper;
using FilmRentalStore.DTO;
using FilmRentalStore.Models;
using Microsoft.EntityFrameworkCore;
using System.Text.RegularExpressions;

namespace FilmRentalStore.Services
{
    public class PaymentService : IPaymentRepository
    {
        public readonly Sakila12Context _context;
        public readonly IMapper _mapper;

        public PaymentService(Sakila12Context sakila, IMapper mapper)
        {
            _context = sakila;
            _mapper = mapper;
        }


        #region Get Payment

        public async Task<List<PaymentDTO>> GetPayment()
        {
            var List = await _context.Payments.ToListAsync();
            var DTOList = _mapper.Map<List<PaymentDTO>>(List);
            return DTOList;
        }

        #endregion


        #region MakePayment

        public async Task<List<PaymentDTO>> MakePayment(int paymentId, decimal amount)
        {
            var pay = await _context.Payments.FirstOrDefaultAsync(s => s.PaymentId == paymentId);

            if (pay == null)
            {
                return null;
            }

            pay.Amount = amount;
            await _context.SaveChangesAsync();

            //To fetch the updated details list
            var updatedlist = await _context.Payments.Where(s => s.StaffId == pay.PaymentId).ToListAsync();

            return _mapper.Map<List<PaymentDTO>>(updatedlist);
        }

        #endregion


        #region GetCumulativeRevenueOfAllStores

        public async Task<List<PaymentDTO>> GetCumulativeRevenueOfAllStores()
        {
            var cumulativeRevenue = await (from payment in _context.Payments
                                           join staff in _context.Staff on payment.StaffId equals staff.StaffId
                                           group payment by new { staff.StoreId, payment.PaymentDate.Date } into grouped
                                           select new PaymentDTO
                                           {
                                               
                                               StaffId = grouped.Key.StoreId,  
                                               PaymentDate = grouped.Key.Date,
                                               Amount = grouped.Sum(p => p.Amount), 
                                               CustomerId = grouped.FirstOrDefault().CustomerId, 
                                               RentalId = grouped.FirstOrDefault().RentalId, 
                                               LastUpdate = DateTime.Now 
                                           }).ToListAsync();

            return cumulativeRevenue;
        }


        #endregion


        #region GetCumulativeRevenueForAStore

        public async Task<List<PaymentDTO>> GetCumulativeRevenueForAStore(int storeId)
        {
            var result = await (from p in _context.Payments
                                join s in _context.Staff on p.StaffId equals s.StaffId
                                where s.StoreId == storeId  // Filter by storeId (string)
                                group p by new { s.StoreId, p.PaymentDate.Date } into grouped
                                select new PaymentDTO
                                {
                                    StaffId = grouped.Key.StoreId, 
                                    PaymentDate = grouped.Key.Date, 
                                    Amount = grouped.Sum(p => p.Amount), 
                                    CustomerId = grouped.FirstOrDefault().CustomerId,  
                                    RentalId = grouped.FirstOrDefault().RentalId, 
                                    LastUpdate = DateTime.Now
                                }).ToListAsync();

            return result;
        }

        #endregion


        #region GetPaymentsByFilmTitle

        public async Task<List<PaymentDTO>> GetPaymentsByFilmTitle(string filmTitle)
        {
            var result = await (from p in _context.Payments
                                join r in _context.Rentals on p.RentalId equals r.RentalId
                                join i in _context.Inventories on r.InventoryId equals i.InventoryId
                                join f in _context.Films on i.FilmId equals f.FilmId
                                where f.Title == filmTitle
                                select new PaymentDTO
                                {
                                    PaymentId = p.PaymentId,
                                    CustomerId = p.CustomerId,
                                    StaffId = p.StaffId,
                                    RentalId = p.RentalId,
                                    Amount = p.Amount,
                                    PaymentDate = p.PaymentDate,
                                    LastUpdate = p.LastUpdate
                                }).ToListAsync();

            return result;
        }

        #endregion


        #region GetCumulativeRevenueStoreWise

        public async Task<List<PaymentDTO>> GetCumulativeRevenueStoreWise(int storeid)
        {
            try
            {
                var result = await (from payment in _context.Payments
                                      join staff in _context.Staff on payment.StaffId equals staff.StaffId
                                      join store in _context.Stores on staff.StoreId equals store.StoreId
                                      where store.StoreId == storeid  // Filter by the provided storeId
                                      select new PaymentDTO
                                      {
                                          PaymentId = payment.PaymentId,
                                          CustomerId = payment.CustomerId,
                                          StaffId = payment.StaffId,
                                          RentalId = payment.RentalId,
                                          Amount = payment.Amount,
                                          PaymentDate = payment.PaymentDate,
                                          LastUpdate = payment.LastUpdate
                                      }).ToListAsync(); 


                return result;
            }
            catch (Exception ex)
            {
                // Handle the exception appropriately
                throw new Exception("An error occurred while fetching cumulative revenue.", ex);
            }
        }


        #endregion


        #region GetCumulativeRevenueAllFilmsByStore

        public async Task<List<PaymentDTO>> GetCumulativeRevenueAllFilmsByStore()
        {
            try
            {
                var result = await (from payment in _context.Payments
                                    join rental in _context.Rentals on payment.RentalId equals rental.RentalId
                                    join inventory in _context.Inventories on rental.InventoryId equals inventory.InventoryId
                                    join film in _context.Films on inventory.FilmId equals film.FilmId
                                    join staff in _context.Staff on payment.StaffId equals staff.StaffId
                                    join store in _context.Stores on staff.StoreId equals store.StoreId
                                    group new { payment, film, store } by new { film.FilmId, store.StoreId, film.Title } into grouped
                                    select new PaymentDTO
                                    {
                                        
                                        PaymentId = grouped.FirstOrDefault().payment.PaymentId, 
                                        CustomerId = grouped.FirstOrDefault().payment.CustomerId,
                                        StaffId = grouped.FirstOrDefault().payment.StaffId, 
                                        RentalId = grouped.FirstOrDefault().payment.RentalId, 
                                        Amount = grouped.Sum(g => g.payment.Amount), 
                                        PaymentDate = grouped.FirstOrDefault().payment.PaymentDate, 
                                        LastUpdate = grouped.FirstOrDefault().payment.LastUpdate 
                                    }).ToListAsync();

                return result;
            }
            catch (Exception ex)
            {
                throw new Exception("Error occurred while calculating cumulative revenue.", ex);
            }
        }


        #endregion

    }
}
