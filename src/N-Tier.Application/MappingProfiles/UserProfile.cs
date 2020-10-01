using AutoMapper;
using N_Tier.Application.Models.User;
using N_Tier.DataAccess.Identity;

namespace N_Tier.Application.MappingProfiles
{
    public class UserProfile : Profile
    {
        public UserProfile()
        {
            CreateMap<CreateUserModel, ApplicationUser>();
        }
    }
}
