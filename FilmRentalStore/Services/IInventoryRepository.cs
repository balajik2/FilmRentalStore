using FilmRentalStore.DTO;

namespace FilmRentalStore.Services
{
    public interface IInventoryRepository
    {
         Task AddFilm(InventoryDTO inventoryDTO);
        Task<List<KeyValuePair<string, int>>> CountOfFilmasync();
     
        Task<List<object>> GetAllFilmsinaStore(int storeid);

        Task<List<Object>> Getinventoryforallfilms(int filmid);

        Task<object> GetFilmCountInStore(int filmid, int storeid);

    }
}
