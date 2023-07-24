using VHV_FileSaver.ViewModels.JWTModels;
using VHV_FileSaver.Services.Abstract;

namespace VHV_FileSaver.Services.Interfaces
{
    public interface ITokenService : IService
    {
        public TokenViewModel GenerateAccessToken(string email, string name, int id, string[] roleNames, bool isLogin = false);
    }
}