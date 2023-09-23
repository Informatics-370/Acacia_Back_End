using Acacia_Back_End.Core.Models;
using Acacia_Back_End.Core.Models.CustomerOrders;
using Acacia_Back_End.Core.Models.CustomerReturns;
using Acacia_Back_End.Core.Models.SupplierOrders;
using Acacia_Back_End.Core.Models.SupplierReturns;
using Acacia_Back_End.Core.Specifications;
using Acacia_Back_End.Core.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace Acacia_Back_End.Core.Interfaces
{
    public interface IGenericRepository<T> where T : BaseEntity
    {
        Task<T> GetByIdAsync(int id);
        Task<IReadOnlyList<T>> ListAllAsync();

        Task<bool> AddEntityList(IReadOnlyList<T> entities);

        Task<bool> AddEntity(T entity);

        Task<bool> AddProductType(ProductType type);

        Task<bool> AddProductCategory(ProductCategory category);

        Task<bool> UpdateProductCategory(ProductCategory category);

        Task<bool> UpdateProductType(ProductType type);

        Task<bool> RemoveEntity(int id);

        Task<bool> UpdateEntity(T entity);

        //Products
        Task<IReadOnlyList<Product>> GetProductsAsync(ProductSpecParams pageParams);
        Task<Product> GetProductByIdAsync(int id);
        Task<List<Product>> VerifyProductList(List<Product> products);

        //GiftBoxes
        Task<IReadOnlyList<GiftBox>> GetGiftBoxesAsync(SpecParams specParams);
        Task<GiftBox> GetGiftBoxByIdAsync(int id);

        // FAQs
        Task<IReadOnlyList<FAQ>> GetFaqsAsync(SpecParams specParams);

        // Suppliers
        Task<IReadOnlyList<Supplier>> GetSuppliersAsync(SpecParams specParams);

        // Vats
        Task<IReadOnlyList<Vat>> GetVatsAsync(VatSpecParams specParams);
        Task<Vat> GetActiveVat();

        //Promotions
        Task<IReadOnlyList<Promotion>> GetPromotionsAsync(PromotionSpecParams specParams);
        Task<Promotion> GetPromotionByIdAsync(int id);
        Task<bool> RemovePromotion(int id);

        //Orders
        Task<IReadOnlyList<Order>> GetUserOrdersAsync(string customerEmail, OrderParams searchParams);
        Task<IReadOnlyList<Order>> GetOrdersAsync(OrderParams searchParams);
        Task<Order> GetOrderByIdAsync(int id);
        Task<bool> VerifyStockForSaleOrder(string cartId);
        Task<Order> CreateOrderAsync(string customerRmail, int deliveryMethodId, int orderTypeId, string cartId, OrderAddress shippingAddress);
        Task<IReadOnlyList<DeliveryMethod>> GetDeliveryMethodsAsync();
        Task<IReadOnlyList<OrderType>> GetOrderTypesAsync();
        Task<bool> DispatchOrderAsync(int orderId, string trackingNumber);
        Task<bool> PackageOrder(int orderId);

        // Define your ProductReview functions here and Implement them in the GenericRepository.cs file
        Task<IReadOnlyList<ProductReview>> GetProductReviewsAsync(ReviewParams specParams); // Customer +  Manager (need on more)
        Task<ProductReview> GetProductReviewByIdAsync(int id); // Customer +  Manager
        Task<bool> CreateProductReviewAsync(ProductReview prodReview); // Customer
        Task<bool> FlagProductReviewAsync(int idl); // Customer +  Manager (need on more)
        Task<bool> UnFlagProductReviewAsync(int id, string ManagerEmail); // Customer +  Manager (need on more)

        // Write-offs
        Task<IReadOnlyList<WriteOff>> GetWriteOffsAsync(WriteOffParams writeoffParams);
        Task<WriteOff> GetWriteOffByIdAsync(int id);
        Task<bool> CreateWriteOff(WriteOff wrtieoff);

        // Customer Returns 
        Task<CustomerReturn> LogCustomerReturnAsync(int orderId, string customerEmail, string ReturnDescription, List<ReturnItem> returnedItems);
        Task<bool> VerifyReturnRequest(int orderId, string customerEmail, List<ReturnItem> returnedItems);
        Task<IReadOnlyList<CustomerReturn>> GetReturnsLogAsync(SpecParams searchParams);
        Task<CustomerReturn> GetCustomerReturnByIdAsync(int id);

        //Supplier Orders
        Task<IReadOnlyList<SupplierOrder>> GetSupplierOrdersAsync(OrderParams searchParams);
        Task<SupplierOrder> GetSupplierOrderByIdAsync(int id);
        Task<SupplierOrder> CreateSupplierOrderAsync(string managerEmail, int supplierId, List<SupplierOrderItemVM> orderItems);
        Task<bool> ApproveSupplierOrder(int orderId, List<SupplierOrderItemVM> orderItems);
        Task<bool> ConfirmSupplierOrderDelivery(int orderId, List<SupplierOrderItemVM> orderItems);
        Task<bool> ConfirmSupplierOrderPayment(int orderId, string InvoiceUrl, string ProofOfPaymentUrl);
        Task<bool> CancelSupplierOrder(int orderId);

        // Customer Returns 
        Task<SupplierReturn> LogSupplierReturnAsync(int orderId, string managerEmail, string ReturnDescription, List<SupplierReturnItem> returnedItems);
        Task<bool> VerifySupplierReturnRequest(int orderId, List<SupplierReturnItem> returnedItems);
        Task<IReadOnlyList<SupplierReturn>> GetSupplierReturnsLogAsync(SpecParams searchParams);
        Task<SupplierReturn> GetSupplierReturnByIdAsync(int id);

        // Delivery Method CRUDs
        Task<IReadOnlyList<DeliveryMethod>> GeDeliveryMethodsAsync(SpecParams searchParams);

        // Media CRUDs
        Task<IReadOnlyList<Media>> GetMediaListAsync(SpecParams searchParams);

        // Delete Cascade
        Task<bool> RemoveProduct(int id);
        Task<bool> RemoveProductType(int id);
        Task<bool> RemoveProductCategory(int id);
        Task<bool> RemoveSupplier(int id);
        Task<bool> RemoveGiftBox(int id);
        Task<bool> RemoveDeliveryMethod(int id);
        Task<bool> UpdateDeliveryMethod(DeliveryMethod deliveryMethod);

    }
}
