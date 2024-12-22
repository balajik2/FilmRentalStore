using FilmRentalStore.DTO;

namespace FilmRentalStore.Services
{
    public interface IPaymentRepository
    {
        Task<List<PaymentDTO>> GetPayment();
        Task<List<PaymentDTO>> MakePayment(int paymentId, decimal amount);
        Task<List<PaymentDTO>> GetCumulativeRevenueOfAllStores();
        Task<List<PaymentDTO>> GetCumulativeRevenueForAStore(int storeId);
        Task<List<PaymentDTO>> GetPaymentsByFilmTitle(string filmTitle);
        Task<List<PaymentDTO>> GetCumulativeRevenueStoreWise(int storeid);
        Task<List<PaymentDTO>> GetCumulativeRevenueAllFilmsByStore();
    }
}
