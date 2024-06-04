using AuthService.Data;
using AuthService.Data.Auth;
using AuthService.Data.EmailModel;
using AuthService.Data.UserDatas.DTOs;
using AuthService.Data.UserDatas.Model;
using AuthService.Helpers;
using AuthService.Services.UserManagement.Interface;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace AuthService.Services.UserManagement.Implementation
{
    public class UserService : IUserService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IConfiguration _configuration;
        private readonly AppDbContext _context;
        private readonly JwtConfig _jwtConfig;
        private readonly ILogger<UserService> _logger;
        private readonly IMailSender _mailSender;


        public UserService(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            IConfiguration configuration,
            JwtConfig jwtConfig,
            AppDbContext context,
            ILogger<UserService> logger,
            IMailSender mailSender)
        {
            _userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
            _signInManager = signInManager ?? throw new ArgumentNullException(nameof(signInManager));
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
            _jwtConfig = jwtConfig ?? throw new ArgumentNullException(nameof(jwtConfig));
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _logger = logger ?? throw new ArgumentNullException();
            _mailSender = mailSender;
            // _userService = userService;
        }

        public async Task<RegisterResponse> RegisterAsync(RegisterDto model)
        {
            try
            {
                if (model == null)
                {
                    throw new ArgumentNullException(nameof(model));
                }

                _logger.LogInformation("Starting user registration process for email: {Email}", model.Email);

                var emailExists = await _context.ApplicationUsers
                    .Where(x => x.Email == model.Email)
                    .FirstOrDefaultAsync();

                if (emailExists != null)
                {
                    _logger.LogWarning("Email {Email} already exists.", model.Email);
                    return new RegisterResponse
                    {
                        Success = false,
                        Message = "Email already exists."
                    };
                }

                var user = new ApplicationUser
                {
                    FirstName = model.FirstName,
                    LastName = model.LastName,
                    Email = model.Email,
                    IsSeller = model.IsSeller,
                    UserName = model.Email // Set UserName to Email
                };

                var result = await _userManager.CreateAsync(user, model.Password);

                if (result.Succeeded)
                {
                    _logger.LogInformation("User {Email} registered successfully.", user.Email);

                    var userDetails = new UserDetails
                    {
                        Id = user.Id,
                        FirstName = user.FirstName,
                        LastName = user.LastName,
                        Email = user.Email
                    };

                    return new RegisterResponse
                    {
                        Success = true,
                        Message = "User registered successfully.",
                        IdentityResult = result,
                        UserDetails = userDetails
                    };
                }
                else
                {
                    var errorMessage = result.Errors.Any(e => e.Code == "DuplicateUserName")
                        ? "Username is already taken."
                        : string.Join("; ", result.Errors.Select(e => e.Description));

                    _logger.LogWarning("User registration failed for email {Email}: {Errors}", model.Email, errorMessage);

                    return new RegisterResponse
                    {
                        Success = false,
                        Message = errorMessage,
                        IdentityResult = result
                    };
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred during user registration.");
                return new RegisterResponse
                {
                    Success = false,
                    Message = $"Error registering user: {ex.Message}"
                };
            }
        }


        public async Task<Result<LoginResponseDto>> Login(LoginDto model)
        {
            try
            {
                _logger.LogInformation("Attempting to find user by email: {Email}", model.Email);

                ApplicationUser existingUser = await _userManager.FindByEmailAsync(model.Email);
                if (existingUser == null)
                {
                    _logger.LogWarning("User with email '{Email}' not found.", model.Email);
                    return Result<LoginResponseDto>.Failure("Invalid email or password.");
                }

                SignInResult loginResult = await _signInManager.PasswordSignInAsync(existingUser, model.Password, isPersistent: true, lockoutOnFailure: false);
                if (!loginResult.Succeeded)
                {
                    _logger.LogWarning("Invalid password for user with email '{Email}'.", model.Email);
                    return Result<LoginResponseDto>.Failure("Invalid email or password.");
                }

                var userRefreshToken = await PersistRefreshToken(existingUser, GenerateRefreshToken());
                LoginResponseDto response = new LoginResponseDto()
                {
                    Token = await GenerateToken(existingUser),
                    UserId = existingUser.Id,
                    FullName = $"{existingUser.FirstName} {existingUser.LastName}",
                    Email = existingUser.Email,
                    RefreshToken = userRefreshToken.RefreshToken,
                    RefreshTokenExpiryTime = userRefreshToken.RefreshTokenExpiryTime,
                };
                return Result<LoginResponseDto>.Success(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred during the login process.");
                return Result<LoginResponseDto>.Failure($"An error occurred during the login process: {ex.Message}");
            }
        }



        private async Task<PersistedLogin> PersistRefreshToken(ApplicationUser user, string refreshToken)
        {
            var userRefreshToken = await _context.PersistedLogins.FirstOrDefaultAsync(c => c.UserId == new Guid(user.Id));
            if (userRefreshToken == null)
            {
                userRefreshToken = new PersistedLogin()
                {
                    UserId = new Guid(user.Id),
                    RefreshToken = refreshToken,
                    RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(_jwtConfig.RefreshTokenValidityinDays)
                };
                _context.PersistedLogins.Add(userRefreshToken);
                await _context.SaveChangesAsync();
            }
            else
            {
                userRefreshToken.RefreshToken = refreshToken;
                userRefreshToken.LastUpdatedOn = DateTime.UtcNow;
                userRefreshToken.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(_jwtConfig.RefreshTokenValidityinDays);
                _context.PersistedLogins.Update(userRefreshToken);
                await _context.SaveChangesAsync();
            }

            return userRefreshToken;
        }
        private static string GenerateRefreshToken()
        {
            var randomNumber = new byte[64];
            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(randomNumber);
            return Convert.ToBase64String(randomNumber);
        }


        public async Task<string> GenerateToken(ApplicationUser user)
        {
            try
            {
                List<Claim> claims = new()
        {
            new Claim(ClaimTypes.NameIdentifier, user.Id),
            new Claim(ClaimTypes.Email, user.Email),
            new Claim(ClaimTypes.Name, user.UserName),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new Claim(JwtRegisteredClaimNames.Iss, _jwtConfig.ValidIssuer),
            new Claim(JwtRegisteredClaimNames.Aud, _jwtConfig.ValidAudience),
            new Claim("token_type", "access")
        };

                IList<string> userRoles = await _userManager.GetRolesAsync(user);
                foreach (string role in userRoles)
                {
                    claims.Add(new Claim(ClaimTypes.Role, role));
                }

                SymmetricSecurityKey symmetricSecurityKey = new(Encoding.UTF8.GetBytes(_jwtConfig.SecretKey));

                SecurityTokenDescriptor tokenDescriptor = new()
                {
                    Subject = new ClaimsIdentity(claims),
                    Expires = DateTime.UtcNow.AddHours(24),
                    SigningCredentials = new SigningCredentials(symmetricSecurityKey, SecurityAlgorithms.HmacSha256Signature),
                    Issuer = _jwtConfig.ValidIssuer,
                    Audience = _jwtConfig.ValidAudience
                };

                JwtSecurityTokenHandler tokenHandler = new();
                var token = tokenHandler.CreateToken(tokenDescriptor);

                return tokenHandler.WriteToken(token);
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Error generating token: {ex.Message}", ex);
            }
        }



        public async Task<Result<LoginResponseDto>> RefreshToken(RefreshTokenNewRequestModel tokenModel)
        {
            try
            {
                string? accessToken = tokenModel.AccessToken;
                string? refreshToken = tokenModel.RefreshToken;

                var principal = GetPrincipalFromExpiredToken(accessToken);
                if (principal == null)
                {
                    return Result<LoginResponseDto>.Failure("Invalid access token or refresh token");
                }

                var emailClaim = principal.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email);
                if (emailClaim == null)
                {
                    return Result<LoginResponseDto>.Failure("Invalid access token or refresh token");
                }

                var existingUser = await _userManager.FindByEmailAsync(emailClaim.Value);
                if (existingUser == null)
                {
                    return Result<LoginResponseDto>.Failure("Invalid access token or refresh token");
                }

                var userRefreshToken = await _context.PersistedLogins.FirstOrDefaultAsync(c => c.UserId == new Guid(existingUser.Id));
                if (userRefreshToken == null)
                {
                    return Result<LoginResponseDto>.Failure("Invalid token");
                }

                if (userRefreshToken.RefreshToken != refreshToken || userRefreshToken.RefreshTokenExpiryTime <= DateTime.Now)
                {
                    return Result<LoginResponseDto>.Failure("Invalid access token or refresh token");
                }

                var newAccessToken = await GenerateToken(existingUser);
                var newRefreshToken = GenerateRefreshToken();

                userRefreshToken = await PersistRefreshToken(existingUser, newRefreshToken);

                LoginResponseDto response = new()
                {
                    Token = newAccessToken,
                    UserId = existingUser.Id,
                    FullName = $"{existingUser.FirstName} {existingUser.LastName}",
                    Email = existingUser.Email,
                    RefreshToken = newRefreshToken,
                    RefreshTokenExpiryTime = userRefreshToken.RefreshTokenExpiryTime
                };
                return Result<LoginResponseDto>.Success(response);
            }
            catch (Exception ex)
            {
                // Optionally log the exception here
                return Result<LoginResponseDto>.Failure($"An error occurred while refreshing the token: {ex.Message}");
            }
        }



        private ClaimsPrincipal? GetPrincipalFromExpiredToken(string? token)
        {
            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateAudience = false,
                ValidateIssuer = false,
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtConfig.SecretKey)),
                ValidateLifetime = false
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out SecurityToken securityToken);
            if (securityToken is not JwtSecurityToken jwtSecurityToken || !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
                throw new SecurityTokenException("Invalid token");

            return principal;

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


        public async Task<string> ChangePassword(string email, ChangePassword model)
        {
            try
            {
                ApplicationUser user = await _userManager.FindByEmailAsync(email);
                if (user == null) return "User doesn't exist";

                if (!await _userManager.CheckPasswordAsync(user, model.OldPassword))
                    return "Old password is incorrect";

                var result = await _userManager.ChangePasswordAsync(user, model.OldPassword, model.NewPassword);

                if (!result.Succeeded)
                {
                    string errors = string.Join(" ", result.Errors.Select(e => e.Description));
                    return errors;
                }
                var emailModel = new EmailMessageModel
                {
                    Email = email,
                    FirstName = user.FirstName,
                    LastName = user.LastName
                };

                await _mailSender.ChangePassword(emailModel);
                return "User password successfully changed";
            }
            catch (Exception ex)
            {
                return $"An error occurred: {ex.Message}";
            }
        }

        public async Task<Result<LogoutRequestDto>> Logout(string accessToken)
        {
            try
            {
                var tokenHandler = new JwtSecurityTokenHandler();
                var token = tokenHandler.ReadJwtToken(accessToken);

                // Extract the user ID from the 'nameid' claim
                var userIdClaim = token.Claims.FirstOrDefault(claim => claim.Type == JwtRegisteredClaimNames.NameId);
                if (userIdClaim == null)
                {
                    _logger.LogWarning("The token does not contain a 'nameid' claim.");
                    return Result<LogoutRequestDto>.Failure("Invalid token: 'nameid' claim is missing.");
                }

                var userId = userIdClaim.Value;
                var user = await _context.Users.FindAsync(userId);

                if (user == null)
                {
                    _logger.LogWarning("User not found for user ID: {UserId}", userId);
                    return Result<LogoutRequestDto>.Failure("Invalid token or user not found.");
                }

                var refreshToken = await _context.RefreshTokens.FirstOrDefaultAsync(rt => rt.Token == accessToken);
                if (refreshToken != null)
                {
                    _context.RefreshTokens.Remove(refreshToken);
                    await _context.SaveChangesAsync();
                }

                var logoutRequestDto = new LogoutRequestDto { AccessToken = accessToken };
                return Result<LogoutRequestDto>.Success(logoutRequestDto, "Logout successful.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred during logout.");
                return Result<LogoutRequestDto>.Failure("An error occurred during logout.");
            }
        }

    }
}
