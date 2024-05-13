using AuthService.Data.DTOs;
using AuthService.Data.Model;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;


namespace AuthService.Services.Interface
{
    public interface IUserService
    {
        Task<IdentityResult> RegisterAsync(RegisterDto model);
        Task<SignInResult> LoginAsync(LoginDto model);
        Task<UserDto> GetUserByEmailAsync(string email);
    }
}


