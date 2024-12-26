using FilmRentalStore.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
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
            //dynamic user = null;
            //string roleName = null;

            var ruser = _context.Rusers
                .FromSqlInterpolated($"EXEC GetRuserByCredentials @Username = {username}, @Password = {password}")
                .AsEnumerable() 
                .FirstOrDefault();

            
            if (ruser != null)
            {
                ruser.Role = _context.Roles.FirstOrDefault(r => r.Id == ruser.RoleId);
            }

            //if (ruser != null)
            //{
            //    user = ruser;
            //    roleName = ruser.Role.Name;
            //}
            
            if (ruser == null) return null;
            Console.WriteLine(ruser.Username);
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_configuration["Jwt:Key"]);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[] {
                new Claim(ClaimTypes.Name, ruser.Username),
                new Claim(ClaimTypes.Role, ruser.Role.Name)
            }),
                Expires = DateTime.UtcNow.AddDays(7),
                Audience = _configuration["Jwt:Audience"],  // Ensure the Audience is set
                Issuer = _configuration["Jwt:Issuer"],      // Ensure the Issuer is set
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            Console.WriteLine("Logged in Successfully !");
            return tokenHandler.WriteToken(token);
            
        }
        
    }

}
