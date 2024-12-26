using AutoMapper;
using FilmRentalStore.DTO;
using FilmRentalStore.Models;

namespace FilmRentalStore.Services
{
    public class AddUserService : IAdduserRepository
    {
        private readonly Sakila12Context _context;

        private readonly IMapper _mapper;

        public AddUserService(Sakila12Context context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }


        #region AddUser
        public async Task AddUser(AddUserDTO user)
        {
            var userData = _mapper.Map<Ruser>(user);
            _context.Rusers.Add(userData);
            await _context.SaveChangesAsync();
        }
        #endregion
    }
}
