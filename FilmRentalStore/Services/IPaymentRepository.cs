using FilmRentalStore.DTO;

namespace FilmRentalStore.Services
{
    public interface IPaymentRepository
    {
        Task<List<PaymentDTO>> MakePayment(int paymentId, decimal amount);
        Task<List<PaymentDTO>> GetCumulativeRevenueOfAllStores();
        Task<List<PaymentDTO>> GetCumulativeRevenueForAStore(int storeId);
        Task<List<PaymentWithFilmDTO>> GetPaymentsByFilmTitle(string filmTitle);
        Task<List<PaymentWithAddressDTO>> GetCumulativeRevenueStoreWise(int storeid);
        Task<List<PaymentWithFilmDTO>> GetCumulativeRevenueAllFilmsByStore();
    }
}
