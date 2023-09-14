using AutoMapper;
using Acacia_Back_End.Core.Models;
using Acacia_Back_End.Core.ViewModels;
using Acacia_Back_End.Core.Models.Identities;
using SQLitePCL;
using Acacia_Back_End.Infrastructure.Data;
using NPOI.Util.Collections;
using Org.BouncyCastle.Crypto;
using System.Net.NetworkInformation;
using Acacia_Back_End.Core.Models.CustomerOrders;
using Microsoft.AspNetCore.Identity;
using NPOI.SS.Formula.Functions;
using Acacia_Back_End.Core.Models.SupplierOrders;
using Acacia_Back_End.Core.Models.CustomerReturns;
using Acacia_Back_End.Core.Models.SupplierReturns;

namespace Acacia_Back_End.Helpers
{
    public class MappingProfiles:Profile
    {
        public MappingProfiles()
        {
            CreateMap<Product, ProductVM>()
                .ForMember(d => d.ProductCategory, o => o.MapFrom(s => s.ProductCategory.Name))
                .ForMember(d => d.ProductType, o => o.MapFrom(s => s.ProductType.Name))
                .ForMember(d => d.Supplier, o => o.MapFrom(s => s.Supplier.Name))
                .ForMember(d => d.Price, o => o.MapFrom(s => s.GetPrice()))
                .ForMember(d => d.Promotion, o => o.MapFrom(s => s.Promotion.IsActive ? s.Promotion.Percentage : 0))
            .ForMember(d => d.PictureUrl, o => o.MapFrom<ProductUrlResolver>());


            CreateMap<AppUser, UserVM>()
                .ForMember(d => d.ProfilePicture, o => o.MapFrom<ProfilePictureURLResolver>());


            CreateMap<SupplierOrder, SupplierOrderVM>();
            CreateMap<GiftBox, ListGiftBoxVM>()
                .ForMember(d => d.Price, o => o.MapFrom(s => s.PriceHistory.OrderByDescending(pp => pp.StartDate).First().Price))
                .ForMember(d => d.PackagingCosts, o => o.MapFrom(s => s.PriceHistory.OrderByDescending(pp => pp.StartDate).First().PackagingCosts))
                        .ForMember(d => d.GiftBoxImage, o => o.MapFrom<GiftBoxListUrlResolver>());
            CreateMap<GiftBox, GiftBoxVM>().ForMember(d => d.Products, o => o.MapFrom(s => s.Products))
                .ForMember(d => d.Price, o => o.MapFrom(s => s.PriceHistory.OrderByDescending(pp => pp.StartDate).First().Price))
                .ForMember(d => d.PackagingCosts, o => o.MapFrom(s => s.PriceHistory.OrderByDescending(pp => pp.StartDate).First().PackagingCosts))
                .ForMember(d => d.GiftBoxImage, o => o.MapFrom<GiftBoxUrlResolver>());
            CreateMap<Promotion, PromotionVM>().ForMember(d => d.Products, o => o.MapFrom(s => s.Products));
            CreateMap<Supplier, SupplierVM>().ReverseMap();

            CreateMap<Vat, VatVM>().ReverseMap();
            CreateMap<Promotion, ListPromotionVM>();
            CreateMap<Promotion, PromotionVM>();

            CreateMap<CreateProductReviewVM, ProductReview>();
            CreateMap<ProductReview, ProductReviewVM>()
                .ForMember(d => d.Product, o => o.MapFrom(s => s.Product.Name));

            CreateMap<OrderAddress, OrderAddressVM>().ReverseMap();

            CreateMap<WriteOff, WriteOffVM>()
                .ForMember(d => d.Product, o => o.MapFrom(s => s.Product.Name));

            CreateMap<Order, OrderVM>()
                .ForMember(d => d.DeliveryMethod, o => o.MapFrom(s => s.DeliveryMethod.Name))
                .ForMember(d => d.ShippingPrice, o => o.MapFrom(s => s.DeliveryMethod.Price))
                .ForMember(d => d.OrderType, o => o.MapFrom(s => s.OrderType.Name));

            CreateMap<OrderItem, OrderItemVM>()
                .ForMember(d => d.ProductId, o => o.MapFrom(s => s.ItemOrdered.ProductItemId))
                .ForMember(d => d.ProductName, o => o.MapFrom(s => s.ItemOrdered.ProductName))
                .ForMember(d => d.PictureUrl, o => o.MapFrom(s => s.ItemOrdered.ProductUrl))
            .ForMember(d => d.PictureUrl, o => o.MapFrom<OrderItemUrlResolver>());

            CreateMap<SupplierOrderItem, SupplierOrderItemVM>()
                .ForMember(d => d.ProductId, o => o.MapFrom(s => s.ItemOrdered.ProductItemId))
                .ForMember(d => d.ProductName, o => o.MapFrom(s => s.ItemOrdered.ProductName))
                .ForMember(d => d.PictureUrl, o => o.MapFrom(s => s.ItemOrdered.ProductUrl))
                .ForMember(d => d.PictureUrl, o => o.MapFrom<SupplierOrderItemUrlResolver>());

            CreateMap<CustomerReturn, CustomerReturnVM>();
            CreateMap<SupplierReturn, SupplierReturnVM>();
            CreateMap<ReturnItem, ReturnItemVM>().ReverseMap();
            CreateMap<SupplierReturnItem, SupplierReturnItemVM>().ReverseMap();


            CreateMap<ProductVM, Product>()
                .ForMember(d => d.ProductCategory, o => o.Ignore())
                .ForMember(d => d.ProductType, o => o.Ignore())
                .ForMember(d => d.PictureUrl, o => o.Ignore())
                .ForMember(d => d.ProductCategoryId, o => o.MapFrom(s => 1))
                .ForMember(d => d.ProductTypeId, o => o.MapFrom(s => 1));

            CreateMap<User_Address, User_AddressVM>().ReverseMap();
            CreateMap<CustomerCartVM, CustomerBasket>();
            CreateMap<CartItemVM, BasketItem>();
            CreateMap<CustomerWishlistVM, CustomerWishlist>();
            CreateMap<WishlistItemVM, WishlistItem>();
            CreateMap<Company, CompanyVM>();

        }
    }
}
