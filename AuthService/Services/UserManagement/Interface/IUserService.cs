using AuthService.Data.UserDatas.Model;
using AuthService.Data.UserDatas.DTOs;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;
using AuthService.Helpers;


namespace AuthService.Services.UserManagement.Interface
{
    public interface IUserService
    {
        //Task<IdentityResult> RegisterAsync(RegisterDto model);
        Task<RegisterResponse> RegisterAsync(RegisterDto model);

        Task<UserDto> GetUserByEmailAsync(string email);
        Task<Result<LoginResponseDto>> Login(LoginDto model);
        Task<Result<LoginResponseDto>> RefreshToken(RefreshTokenNewRequestModel tokenModel);
        //Task<string> RegisterUserAsync(RegisterDto userRegistration, string role);
    }
}


