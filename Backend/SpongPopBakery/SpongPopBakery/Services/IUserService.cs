using SpongPopBakery.DTOs;
using SpongPopBakery.Models;

namespace SpongPopBakery.Services
{
    public interface IUserService
    {
        Task<User> Register(UserRegisterDto userRegisterDto);
        Task<User> Login(UserLoginDto userLoginDto);
        string GenerateJwtToken(User user);
        Task<IEnumerable<User>> GetAllUsers();
    }
}
