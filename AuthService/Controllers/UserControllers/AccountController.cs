using AuthService.Data.UserDatas.Model;
using AuthService.Data.UserDatas.DTOs;
using AuthService.Services.UserManagement.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;
using AuthService.Helpers;
using Microsoft.AspNetCore.Authorization;
using AuthService.Data.Auth;

namespace AuthService.Controllers.UserControllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IMailSender _mailSender;


        public AccountController(IUserService userService, IMailSender mailSender)
        {
            _userService = userService;
            _mailSender = mailSender;
        }


        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterDto model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var response = await _userService.RegisterAsync(model);

            if (response.Success)
            {
                return Ok(new
                {
                    response.Success,
                    response.Message,
                    User = response.UserDetails
                });
            }

            return BadRequest(new
            {
                response.Success,
                response.Message,
                Errors = response.IdentityResult.Errors
            });
        }

        [HttpPost]
        [Route("login")]
        [AllowAnonymous]
        public async Task<IActionResult> Login([FromBody] LoginDto model)
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values.SelectMany(c => c.Errors.Select(d => d.ErrorMessage)).ToList();
                var modelResponse = Result<LoginResponseDto>.ValidationError(errors);
                return BadRequest(modelResponse);
            }
            var response = await _userService.Login(model);
            if (!response.Succeeded) return BadRequest(response);
            return Ok(response);
        }

        [HttpPost]
        [Route("refresh_token")]
        [AllowAnonymous]
        public async Task<IActionResult> RefreshToken([FromBody] RefreshTokenNewRequestModel model)
        {

            var response = await _userService.RefreshToken(model);
            if (!response.Succeeded) return BadRequest(response);
            return Ok(response);
        }

        [HttpPost("change_password/{email}")]
        public async Task<IActionResult> ChangePassword([FromRoute] string email, [FromBody] ChangePassword model)
        {

            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values.SelectMany(c => c.Errors.Select(d => d.ErrorMessage)).ToList();
                return BadRequest(errors);
            }

            var result = await _userService.ChangePassword(email, model);
            if (result != null)
            {
                return Ok(new JsonMessage<string>()
                {
                    status = true,
                    success_message = "Successfully Changed Password"
                });

            }
            return Ok(new JsonMessage<string>()
            {
                error_message = result,
                status = false
            });
        }


    }
}


