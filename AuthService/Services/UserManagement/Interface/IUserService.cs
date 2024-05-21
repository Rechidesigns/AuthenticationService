using AuthService.Data.UserDatas.Model;
using AuthService.Data.UserDatas.DTOs;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;


namespace AuthService.Services.UserManagement.Interface
{
    public interface IUserService
    {
        Task<IdentityResult> RegisterAsync(RegisterDto model);
        Task<SignInResult> LoginAsync(LoginDto model);
        Task<UserDto> GetUserByEmailAsync(string email);
    }
}


