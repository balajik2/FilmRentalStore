using FilmRentalStore.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace FilmRentalStore.Services
{
    public class AuthService : IAuthRepository
    {

        private readonly Sakila12Context _context;
        private readonly IConfiguration _configuration;

        public AuthService(Sakila12Context context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }
        public string Authenticate(string username, string password)
        {
            var user = _context.Staff.Include(u => u.Role).FirstOrDefault(u => u.Username == username && u.Password == password);
            if (user == null) return null;
            Console.WriteLine(user.FirstName);
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_configuration["Jwt:Key"]);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[] {
                new Claim(ClaimTypes.Name, user.Username),
                new Claim(ClaimTypes.Role, user.Role.Name)
            }),
                Expires = DateTime.UtcNow.AddDays(7),
                Audience = _configuration["Jwt:Audience"],  // Ensure the Audience is set
                Issuer = _configuration["Jwt:Issuer"],      // Ensure the Issuer is set
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
            //return "";
        }
    }
}
