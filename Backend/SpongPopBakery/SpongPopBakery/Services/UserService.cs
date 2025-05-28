using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using SpongPopBakery.Data;
using SpongPopBakery.DTOs;
using SpongPopBakery.Models;
using SpongPopBakery.Utilities;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace SpongPopBakery.Services
{
    // Services/UserService.cs
    public class UserService : IUserService
    {
        private readonly BakeryDbContext _context;
        private readonly IConfiguration _config;

        public UserService(BakeryDbContext context, IConfiguration config)
        {
            _context = context;
            _config = config;
        }

        public async Task<User> Register(UserRegisterDto userRegisterDto)
        {
            // Check if username exists
            if (await _context.Users.AnyAsync(x => x.Username == userRegisterDto.Username))
                throw new AppException("Username is already taken");

            // Check if email exists
            if (await _context.Users.AnyAsync(x => x.Email == userRegisterDto.Email))
                throw new AppException("Email is already registered");

            // Create password hash
            CreatePasswordHash(userRegisterDto.Password, out var passwordHash, out var passwordSalt);

            // Create new user
            var user = new User
            {
                Username = userRegisterDto.Username,
                Email = userRegisterDto.Email,
                PasswordHash = passwordHash,
                PasswordSalt = passwordSalt,
                Role = userRegisterDto.Role
            };

            // Save user
            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return user;
        }

        public async Task<User> Login(UserLoginDto userLoginDto)
        {
            var user = await _context.Users.FirstOrDefaultAsync(x => x.Username == userLoginDto.Username);

            if (user == null || !VerifyPasswordHash(userLoginDto.Password, user.PasswordHash, user.PasswordSalt))
                throw new AppException("Username or password is incorrect");

            return user;
        }

        public string GenerateJwtToken(User user)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_config["JwtSettings:SecretKey"]);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.Username),
                new Claim(ClaimTypes.Role, user.Role)
            }),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials(
                    new SymmetricSecurityKey(key),
                    SecurityAlgorithms.HmacSha256Signature),
                Issuer = _config["JwtSettings:Issuer"],
                Audience = _config["JwtSettings:Audience"]
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

        public async Task<IEnumerable<User>> GetAllUsers()
        {
            return await _context.Users.ToListAsync();
        }

        private static void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            using var hmac = new HMACSHA512();
            passwordSalt = hmac.Key;
            passwordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
        }

        private static bool VerifyPasswordHash(string password, byte[] storedHash, byte[] storedSalt)
        {
            using var hmac = new HMACSHA512(storedSalt);
            var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
            return computedHash.SequenceEqual(storedHash);
        }
    }
}
