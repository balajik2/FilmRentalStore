using System.Linq;
using AutoMapper;
using FilmRentalStore.DTO;
using FilmRentalStore.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;


namespace FilmRentalStore.Services
{
    public class InventoryServices : IInventoryRepository
    {
        private readonly Sakila12Context _context;
        public readonly IMapper _mapper;
        public InventoryServices(Sakila12Context context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        #region AddFilm
        /// <summary>
        /// add film id to inventory table
        /// </summary>
        /// <param name="inventoryDTO"></param>
        /// <returns></returns>
        public async Task AddFilm(InventoryDTO inventoryDTO)
        {
            var film = _mapper.Map<Inventory>(inventoryDTO);
            _context.Add(film);
            await _context.SaveChangesAsync();
        }
        #endregion
        #region CountOfFilm
        /// <summary>
        /// display count of all stores in all films
        /// </summary>
        /// <returns></returns>
        public async Task<List<KeyValuePair<string, int>>> CountOfFilmasync()
        {
            var data = await _context.Inventories.GroupBy(i => i.FilmId).Select(g => new KeyValuePair<string, int>
            (g.Select(i => i.Film.Title).FirstOrDefault(),
            g.Count())).ToListAsync();

            return data;
        }
        #endregion
       
        #region GetAllFilminstore
        /// <summary>
        /// get all films in a store
        /// </summary>
        /// <param name="storeid"></param>
        /// <returns></returns>
        public async Task<List<object>> GetAllFilmsinaStore(int storeid)
        {
            var data = await _context.Inventories.Include(i => i.Film).Where(i => i.StoreId == storeid).GroupBy(i => i.Film.Title).Select(group => new
            {
                FilmTitle = group.Key,
                Copies = group.Count()
            }).ToListAsync();
            return _mapper.Map<List<object>>(data);
        }
        #endregion
        #region Getinventoryforallfilms
        /// <summary>
        /// get address and no of copies of films depending on filmid
        /// </summary>
        /// <param name="filmid"></param>
        /// <returns></returns>
        public async Task<List<Object>> Getinventoryforallfilms(int filmid)
        {
            var data = await _context.Inventories.Include(i => i.Store).ThenInclude(s => s.Address).Where(i => i.FilmId == filmid).GroupBy(i => i.Store.Address)
                .Select(g => new
                {
                    StoreAddress = g.Key.Address1 + "," + g.Key.City,
                    copies = g.Count()
                }).ToListAsync();
            return _mapper.Map<List<object>>(data);
        }
        #endregion
        #region GetFilmCountInStore
        /// <summary>
        /// get count of films in a store
        /// </summary>
        /// <param name="filmid"></param>
        /// <param name="storeid"></param>
        /// <returns></returns>
        public async Task<object>GetFilmCountInStore(int filmid,int storeid)
            {
            var data = await _context.Inventories.Include(i => i.Store).ThenInclude(s => s.Address).Where(i => i.FilmId == filmid && i.StoreId == storeid).GroupBy(i => i.Store.Address)
                .Select(g => new
                {
                    StoreAddress = g.Key.Address1 + "," + g.Key.City,
                    copies = g.Count()
                }).FirstOrDefaultAsync();
            return _mapper.Map<object>(data);
            }
        #endregion
    }
}
