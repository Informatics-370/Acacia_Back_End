using Acacia_Back_End.Core.Models;
using Acacia_Back_End.Core.ViewModels;
using AutoMapper;

namespace Acacia_Back_End.Helpers
{
    public class GiftBoxUrlResolver : IValueResolver<GiftBox, GiftBoxVM, string>
    {
        private readonly IConfiguration _config;

        public GiftBoxUrlResolver(IConfiguration config)
        {
            _config = config;
        }

        public string Resolve(GiftBox source, GiftBoxVM destination, string destMember, ResolutionContext context)
        {
            if (!string.IsNullOrEmpty(source.GiftBoxImage))
            {
                return _config["ApiUrl"] + "/" + source.GiftBoxImage;
            }

            return null;
        }
    }

    public class GiftBoxListUrlResolver : IValueResolver<GiftBox, ListGiftBoxVM, string>
    {
        private readonly IConfiguration _config;

        public GiftBoxListUrlResolver(IConfiguration config)
        {
            _config = config;
        }

        public string Resolve(GiftBox source, ListGiftBoxVM destination, string destMember, ResolutionContext context)
        {
            if (!string.IsNullOrEmpty(source.GiftBoxImage))
            {
                return _config["ApiUrl"] + "/" + source.GiftBoxImage;
            }

            return null;
        }
    }
}
