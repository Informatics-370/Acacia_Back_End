using AutoMapper;
using AutoMapper.Execution;
using Acacia_Back_End.Core.Models;
using Microsoft.Extensions.Configuration;
using Acacia_Back_End.Core.ViewModels;

namespace Acacia_Back_End.Helpers
{
    public class ProductUrlResolver : IValueResolver<Product, ProductVM, string>
    {
        private readonly IConfiguration _config;

        public ProductUrlResolver(IConfiguration config)
        {
            _config = config;
        }

        public string Resolve(Product source, ProductVM destination, string destMember, ResolutionContext context)
        {
            if(!string.IsNullOrEmpty(source.PictureUrl))
            {
                return _config["ApiUrl"] + "/" + source.PictureUrl;
            }

            return null;
        }
    }
}
