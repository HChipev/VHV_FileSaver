using VHV_FileSaver.Services.Abstract;
using VHV_FileSaver.ViewModels.JWTModels;
using VHV_FileSaver.ViewModels.ResponseModels;
using VHV_FileSaver.ViewModels.UserModels;

namespace VHV_FileSaver.Services.Interfaces
{
    public interface IIdentityService : IService
    {
        public Task<BaseResponseViewModel> RegisterAsync(UserRegistrationViewModel user);
        public Task<TokensResponseViewModel> LoginAsync(UserLoginViewModel user);
        public TokensResponseViewModel RefreshTokenAsync(TokenViewModel tokens);
    }
}