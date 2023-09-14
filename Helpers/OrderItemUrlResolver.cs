using Acacia_Back_End.Core.Models.CustomerOrders;
using Acacia_Back_End.Core.ViewModels;
using AutoMapper;

namespace Acacia_Back_End.Helpers
{
    public class OrderItemUrlResolver : IValueResolver<OrderItem, OrderItemVM, string>
    {
        private readonly IConfiguration _config;

        public OrderItemUrlResolver(IConfiguration config)
        {
            _config = config;
        }
        public string Resolve(OrderItem source, OrderItemVM destination, string destMember, ResolutionContext context)
        {
            if (!string.IsNullOrEmpty(source.ItemOrdered.ProductUrl))
            {
                return _config["ApiUrl"] + "/" + source.ItemOrdered.ProductUrl;
            }

            return null;
        }
    }
}
