using FoodOracle.API.Data;
using FoodOracle.API.Models;
using FoodOracle.Dtos;
using FoodOracle.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace FoodOracle.Services
{
    public class CustomerService
    {
        private readonly ApplicationDbContext _context;
        private readonly IConfiguration _config;
        public CustomerService(ApplicationDbContext context, IConfiguration config)
        {
            _context = context;
            _config = config;
        }

        public async Task<bool> UserAccountSetup(string username, string password)
        {
            if (_context.Users.Any(u => u.Username == username))
            {
                throw new InvalidOperationException("Username already exists.");
            }

            using var hmac = new HMACSHA512();
            var user = new Customer
            {
                Username = username,
                PasswordSalt = hmac.Key,
                PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password))
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();
            return true;
        }
        private string CreateToken(Customer user)
        {
            var claims = new[]
            {
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(ClaimTypes.Name, user.Username)
        };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]!));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512);

            var token = new JwtSecurityToken(
                issuer: _config["Jwt:Issuer"],
                audience: _config["Jwt:Audience"],
                claims: claims,
                expires: DateTime.Now.AddHours(2),
                signingCredentials: creds);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
        public async Task<bool> DoesUserExist(string username)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Username == username);
            return user != null;
        }
        public async Task<LoginResponseDto> Login(string username, string password)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Username == username);
            if (user == null)
                throw new InvalidOperationException("Invalid username or password.");

            using var hmac = new HMACSHA512(user.PasswordSalt);
            var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
            if (!computedHash.SequenceEqual(user.PasswordHash))
                throw new InvalidOperationException("Invalid username or password.");

            var token = CreateToken(user);

            return new LoginResponseDto
            {
                Token = token,
                UserId = user.Id
            };
        }
    }
}