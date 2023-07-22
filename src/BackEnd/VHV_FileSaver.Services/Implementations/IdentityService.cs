using VHV_FileSaver.Common;
using VHV_FileSaver.Data.Models;
using VHV_FileSaver.Data.Repository;
using VHV_FileSaver.Services.Interfaces;
using VHV_FileSaver.ViewModels.JWTModels;
using VHV_FileSaver.ViewModels.ResponseModels;
using VHV_FileSaver.ViewModels.UserModels;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace VHV_FileSaver.Services.Implementation
{
    public class IdentityService : IIdentityService
    {
        private readonly IRepository<User> _repository;
        private readonly IRepository<Role> _repositoryRole;
        private readonly IRepository<UserRole> _repositoryUserRole;
        private readonly IMapper _mapper;
        private readonly SignInManager<User> _signInManager;
        private readonly UserManager<User> _userManager;
        private readonly ITokenService _tokenService;
        private readonly IConfiguration _configuration;
        private readonly IRepository<UserToken> _repositoryUserToken;
        private readonly ILogger<IdentityService> _logger;

        public IdentityService(IRepository<User> repository, IMapper mapper, UserManager<User> userManager, SignInManager<User> signInManager,
            ITokenService tokenService, IConfiguration configuration, ILogger<IdentityService> logger, IRepository<Role> repositoryRole,
            IRepository<UserRole> repositoryUserRole, IRepository<UserToken> repositoryUserToken)
        {
            _repository = repository;
            _mapper = mapper;
            _userManager = userManager;
            _signInManager = signInManager;
            _tokenService = tokenService;
            _configuration = configuration;
            _logger = logger;
            _repositoryRole = repositoryRole;
            _repositoryUserRole = repositoryUserRole;
            _repositoryUserToken = repositoryUserToken;
        }

        public async Task<BaseResponseViewModel> RegisterAsync(UserRegistrationViewModel viewUser)
        {
            var messages = "";

            var user = _mapper.Map<User>(viewUser);

            user.EmailConfirmed = true;
            try
            {
                var result = await _userManager.CreateAsync(user, viewUser.Password);

                if (result.Succeeded)
                {
                    await _userManager.AddToRoleAsync(user, Roles.Employee);

                    return new BaseResponseViewModel { Success = true, ErrorMessage = "" }; ;
                }
                else
                {
                    foreach (var error in result.Errors)
                    {
                        messages += $"{error.Description}%";
                    }

                    return new BaseResponseViewModel { Success = false, ErrorMessage = messages.Remove(messages.Length - 1) };
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred");
                messages += $"{ex.Message}";
                return new BaseResponseViewModel { Success = false, ErrorMessage = ex.Message };
            }
        }

        public async Task<TokensResponseViewModel> LoginAsync(UserLoginViewModel user)
        {
            var messages = "";

            try
            {
                var dbUser = await _userManager.FindByEmailAsync(user.Email);

                if (dbUser is null)
                {
                    return new TokensResponseViewModel { Success = false, ErrorMessage = "Invalid email or password", Tokens = null };
                }

                var result = await _signInManager.PasswordSignInAsync(user.Email, user.Password, false, false);

                if (result.Succeeded)
                {
                    int id = dbUser.Id;

                    var name = dbUser.FirstName + " " + dbUser.LastName;

                    int roleId = _repositoryUserRole.FindByCondition(ur => ur.UserId == id).RoleId;

                    string[] roleNames = _repositoryRole.FindAllByCondition(r => r.Id == roleId).Select(r => r.Name).ToArray();

                    return new TokensResponseViewModel { Success = true, ErrorMessage = "", Tokens = _tokenService.GenerateAccessToken(user.Email, name, id, roleNames) };
                }
                else
                {
                    return new TokensResponseViewModel { Success = false, ErrorMessage = "Invalid email or password", Tokens = null };
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred");
                messages += $"{ex.Message}";
                return new TokensResponseViewModel { Success = false, ErrorMessage = messages, Tokens = null };
            }
        }


        public TokensResponseViewModel RefreshTokenAsync(TokenViewModel tokens)
        {
            try
            {
                var tokenHandler = new JwtSecurityTokenHandler();
                SecurityToken securityToken;

                var principal = tokenHandler.ValidateToken(tokens.Token, new TokenValidationParameters
                {
                    IssuerSigningKey = new SymmetricSecurityKey
                    (Encoding.UTF8.GetBytes(_configuration["Jwt:Key"])),
                    ValidateIssuer = true,
                    ValidIssuer = _configuration["JWT:Issuer"],
                    ValidateAudience = true,
                    ValidAudience = _configuration["Jwt:Audience"],
                    ValidateLifetime = false,
                    ValidateIssuerSigningKey = true
                }, out securityToken);
                var validatedToken = securityToken as JwtSecurityToken;

                if (validatedToken?.Header.Alg != SecurityAlgorithms.HmacSha256)
                {
                    return new TokensResponseViewModel { Success = false, ErrorMessage = "Invalid algorithm", Tokens = null };
                }

                var nameIdentifier = int.Parse(principal.FindFirst(ClaimTypes.NameIdentifier).Value);

                var user = _repository.FindByCondition(u => u.Id == nameIdentifier);

                if (user is null || _repositoryUserToken.FindByCondition(t => t.UserId == user.Id)?.RefreshTokenExpiryTime < DateTime.UtcNow ||
                    _repositoryUserToken.FindByCondition(t => t.UserId == user.Id).Value != Uri.UnescapeDataString(tokens.RefreshToken))
                {
                    return new TokensResponseViewModel { Success = false, ErrorMessage = "Invalid token", Tokens = null };
                }

                int roleId = _repositoryUserRole.FindByCondition(ur => ur.UserId == user.Id).RoleId;

                string[] roleNames = _repositoryRole.FindAllByCondition(r => r.Id == roleId).Select(r => r.Name).ToArray();

                var name = user.FirstName + " " + user.LastName;

                var newTokens = _tokenService.GenerateAccessToken(user.Email, name, user.Id, roleNames);
                return new TokensResponseViewModel { Success = true, ErrorMessage = "", Tokens = newTokens };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred");
                return new TokensResponseViewModel { Success = false, ErrorMessage = ex.Message, Tokens = null };
            }
        }
    }
}