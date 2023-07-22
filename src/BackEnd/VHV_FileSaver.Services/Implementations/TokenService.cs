using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using VHV_FileSaver.Data.Models;
using VHV_FileSaver.Data.Repository;
using VHV_FileSaver.Services.Interfaces;
using VHV_FileSaver.ViewModels.JWTModels;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace VHV_FileSaver.Services.Implementation
{
    public class TokenService : ITokenService
    {
        private readonly IConfiguration _configuration;
        private readonly IRepository<UserToken> _repository;

        public TokenService(IRepository<UserToken> repository, IConfiguration configuration)
        {
            _repository = repository;
            _configuration = configuration;
        }

        public TokenViewModel GenerateAccessToken(string email, string name, int id, string[] roleNames)
        {
            int.TryParse(_configuration["JWT:TokenValidityInMinutes"], out int tokenValidityInMinutes);
            var expiration = DateTime.UtcNow.AddMinutes(tokenValidityInMinutes);

            var accessToken = CreateJwtToken(
                CreateClaims(email, name, id, roleNames),
                CreateSigningCredentials(),
                expiration
            );

            int.TryParse(_configuration["JWT:RefreshTokenValidityInDays"], out int refreshTokenValidityInDays);
            var refreshTokenExpiration = DateTime.UtcNow.AddDays(refreshTokenValidityInDays);

            var refreshToken = CreateRefreshTokenJwt(
                CreateSigningCredentials(),
                refreshTokenExpiration
            );

            var accessTokenString = new JwtSecurityTokenHandler().WriteToken(accessToken);
            var refreshTokenString = new JwtSecurityTokenHandler().WriteToken(refreshToken);

            var UserRefreshToken = _repository.FindByCondition(t => t.UserId == id);

            if (UserRefreshToken?.Value is null)
            {
                _repository.Add(new UserToken
                {
                    UserId = id,
                    Value = refreshTokenString,
                    LoginProvider = "VHV",
                    Name = name,
                    RefreshTokenExpiryTime = refreshTokenExpiration,
                });
            }
            else
            {
                UserRefreshToken.Value = refreshTokenString;
                UserRefreshToken.RefreshTokenExpiryTime = refreshTokenExpiration;
                _repository.Update(UserRefreshToken);
            }

            _repository.SaveChanges();

            return new TokenViewModel
            {
                Token = accessTokenString,
                RefreshToken = refreshTokenString
            };
        }

        private JwtSecurityToken CreateJwtToken(Claim[] claims, SigningCredentials credentials, DateTime expiration)
        {
            return new JwtSecurityToken(
                _configuration["Jwt:Issuer"],
                _configuration["Jwt:Audience"],
                claims,
                expires: expiration,
                signingCredentials: credentials
            );
        }

        private JwtSecurityToken CreateRefreshTokenJwt(SigningCredentials credentials, DateTime expiration)
        {
            return new JwtSecurityToken(
                _configuration["Jwt:Issuer"],
                _configuration["Jwt:Audience"],
                expires: expiration,
                signingCredentials: credentials
            );
        }

        private Claim[] CreateClaims(string email, string name, int id, string[] roleNames)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Email, email),
                new Claim(ClaimTypes.NameIdentifier, id.ToString()),
                new Claim(ClaimTypes.Name, name)
            };

            foreach (var roleName in roleNames)
            {
                claims.Add(new Claim(ClaimTypes.Role, roleName));
            }

            return claims.ToArray();
        }

        private SigningCredentials CreateSigningCredentials()
        {
            return new SigningCredentials(
                new SymmetricSecurityKey(
                    Encoding.UTF8.GetBytes(_configuration["Jwt:Key"])
                ),
                SecurityAlgorithms.HmacSha256
            );
        }
    }
}
