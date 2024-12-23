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
            //var user = _context.Rusers.Include(u => u.Role).FirstOrDefault(u => u.UserName == username && u.PassWord == password);
            //if (user == null)
            //{
            //    return null;
            //}

            //string hashedPassword = ConvertToSHA256(password);



            dynamic user = null;
            string roleName = null;

            var ruser = _context.Rusers
              .FromSqlInterpolated($@"
               SELECT * 
              FROM Ruser 
              WHERE Username = {username} 
              AND Password = CONVERT(VARCHAR(255), HASHBYTES('SHA2_256', {password}), 2)")
              .Include(u => u.Role)
              .FirstOrDefault();

            if (ruser != null)
            {
                user = ruser;
                roleName = ruser.Role.Name;
            }
            
            if (user == null) return null;
            Console.WriteLine(user.Username);
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
            Console.WriteLine("Logged in Successfully !");
            return tokenHandler.WriteToken(token);
            
        }
        
    }

}
