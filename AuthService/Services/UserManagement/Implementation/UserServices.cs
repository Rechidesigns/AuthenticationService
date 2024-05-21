﻿using AuthService.Data.UserDatas.DTOs;
using AuthService.Data.UserDatas.Model;
using AuthService.Services.UserManagement.Interface;
using Microsoft.AspNetCore.Identity;

namespace AuthService.Services.UserManagement.Implementation
{
    public class UserService : IUserService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;

        public UserService(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        public async Task<IdentityResult> RegisterAsync(RegisterDto model)
        {
            var user = new ApplicationUser
            {
                FirstName = model.FirstName,
                LastName = model.LastName,
                Email = model.Email,
                IsSeller = model.IsSeller
            };
            return await _userManager.CreateAsync(user, model.Password);
        }

        public async Task<SignInResult> LoginAsync(LoginDto model)
        {
            return await _signInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, lockoutOnFailure: false);
        }

        public async Task<UserDto> GetUserByEmailAsync(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user != null)
            {
                return new UserDto
                {
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    Email = user.Email,
                    IsSeller = user.IsSeller
                };
            }
            return null;
        }
    }


}