using AuthService.Data.DTOs;
using AuthService.Data.Model;
using AuthService.Services.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;

namespace AuthService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IUserService _userService;

        public AccountController(IUserService userService)
        {
            _userService = userService;
        }


        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterDto model)
        {
            var result = await _userService.RegisterAsync(model);
            if (result.Succeeded)
            {
                return Ok(new { Message = "User created successfully" });
            }
            return BadRequest(new { Error = "Failed to create user", Errors = result.Errors });
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto model)
        {
            var result = await _userService.LoginAsync(model);
            if (result.Succeeded)
            {
                var userDto = await _userService.GetUserByEmailAsync(model.Email);
                return Ok(new { User = userDto });
            }
            return Unauthorized(new { Error = "Invalid email or password" });
        }

        // Other action methods remain unchanged

        //[HttpPost("logout")]
        //public async Task<IActionResult> Logout()
        //{
        //    await _userService.SignOutAsync();
        //    return Ok(new { Message = "Logout successful" });
        //}
    }
}


