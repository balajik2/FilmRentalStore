using FilmRentalStore.DTO;

namespace FilmRentalStore.Services
{
    public interface IAdduserRepository
    {
        Task AddUser(AddUserDTO user);
    }
}
