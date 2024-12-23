namespace FilmRentalStore.Services
{
    public interface IAuthRepository
    {
        string Authenticate(string username, string password);
    }
}
