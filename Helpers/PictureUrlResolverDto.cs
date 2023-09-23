using Acacia_Back_End.Core.Models;
using Acacia_Back_End.Core.ViewModels;
using AutoMapper;

namespace Acacia_Back_End.Helpers
{
    public class PictureUrlResolverDto : IValueResolver<Product, ProductDto, string>
    {
        private readonly IConfiguration _config;

        public PictureUrlResolverDto(IConfiguration config)
        {
            _config = config;
        }

        public string Resolve(Product source, ProductDto destination, string destMember, ResolutionContext context)
        {
            if (!string.IsNullOrEmpty(source.PictureUrl))
            {
                return _config["ApiUrl"] + "/" + source.PictureUrl;
            }

            return null;
        }
    }
}
