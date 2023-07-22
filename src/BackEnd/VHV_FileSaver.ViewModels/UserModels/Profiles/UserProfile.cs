using AutoMapper;
using VHV_FileSaver.Data.Models;

namespace VHV_FileSaver.ViewModels.UserModels.UserProfiles
{
    public class UserProfile : Profile
    {
        public UserProfile()
        {
            CreateMap<UserRegistrationViewModel, User>()
                .ForMember(u => u.UserName, opt => opt.MapFrom(x => x.Email));
        }
    }
}