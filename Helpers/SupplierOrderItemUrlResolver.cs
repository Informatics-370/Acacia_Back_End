using Acacia_Back_End.Core.Models.CustomerOrders;
using Acacia_Back_End.Core.Models.SupplierOrders;
using Acacia_Back_End.Core.ViewModels;
using AutoMapper;

namespace Acacia_Back_End.Helpers
{
    public class SupplierOrderItemUrlResolver : IValueResolver<SupplierOrderItem, SupplierOrderItemVM, string>
    {
        private readonly IConfiguration _config;

        public SupplierOrderItemUrlResolver(IConfiguration config)
        {
            _config = config;
        }
        public string Resolve(SupplierOrderItem source, SupplierOrderItemVM destination, string destMember, ResolutionContext context)
        {
            if (!string.IsNullOrEmpty(source.ItemOrdered.ProductUrl))
            {
                return _config["ApiUrl"] + "/" + source.ItemOrdered.ProductUrl;
            }

            return null;
        }
    }
}
