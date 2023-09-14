using Acacia_Back_End.Core.Models.Identities;
using Acacia_Back_End.Core.ViewModels;
using AutoMapper;

namespace Acacia_Back_End.Helpers
{
    public class ProfilePictureURLResolver : IValueResolver<AppUser, UserVM, string>
    {
        private readonly IConfiguration _config;

        public ProfilePictureURLResolver(IConfiguration config)
        {
            _config = config;
        }

        public string Resolve(AppUser source, UserVM destination, string destMember, ResolutionContext context)
        {
            if (!string.IsNullOrEmpty(source.ProfilePicture))
            {
                return _config["ApiUrl"] + "/" + source.ProfilePicture;
            }

            return null;
        }
    }
}
