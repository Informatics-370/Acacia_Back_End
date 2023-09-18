using Microsoft.EntityFrameworkCore;
using Acacia_Back_End.Core.Interfaces;
using Acacia_Back_End.Core.Models;
using Acacia_Back_End.Core.Specifications;
using SQLitePCL;
using Acacia_Back_End.Helpers;
using Microsoft.AspNetCore.Identity;
using NPOI.HSSF.Record.Chart;
using Acacia_Back_End.Core.Models.CustomerOrders;
using Microsoft.IdentityModel.Tokens;
using Org.BouncyCastle.Utilities;
using Acacia_Back_End.Core.Models.Identities;
using NPOI.SS.Formula.Functions;
using Acacia_Back_End.Core.ViewModels;
using Acacia_Back_End.Infrastructure.Services;
using static iTextSharp.text.pdf.AcroFields;
using Acacia_Back_End.Core.Models.SupplierOrders;
using System.Text;
using Acacia_Back_End.Core.Models.CustomerReturns;
using Acacia_Back_End.Core.Models.SupplierReturns;

namespace Acacia_Back_End.Infrastructure.Data
{
    public class GenericRepository<T> : IGenericRepository<T> where T : BaseEntity
    {
        private readonly Context _context;
        private readonly IBasketRepository _basketRepository;
        private readonly UserManager<AppUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IEmailService _emailservice;

        public GenericRepository(Context context, IBasketRepository basketRepository, UserManager<AppUser> userManager, RoleManager<IdentityRole> roleManager, IEmailService emailservice)
        {
            _context = context;
            _basketRepository = basketRepository;
            _userManager = userManager;
            _roleManager = roleManager;
            _emailservice = emailservice;
        }

        public async Task<T> GetByIdAsync(int id)
        {
            return await _context.Set<T>().FindAsync(id);
        }

        public async Task<IReadOnlyList<T>> ListAllAsync()
        {
            return await _context.Set<T>().ToListAsync();   
        }

        public async Task<bool> AddEntity(T entity)
        {
            await _context.AddAsync(entity);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> AddProductCategory(ProductCategory category)
        {
            var mycategory = await _context.ProductCategories.Where(x => x.Name.ToLower() == category.Name.ToLower()).FirstOrDefaultAsync();
            if (mycategory != null) return false;

            await _context.ProductCategories.AddAsync(category);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> AddProductType(ProductType type)
        {
            var myType = await _context.ProductTypes.Where(x => x.Name.ToLower() == type.Name.ToLower()).FirstOrDefaultAsync();
            if (myType != null) return false;

            await _context.ProductTypes.AddAsync(type);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> UpdateProductCategory(ProductCategory category)
        {
            var mycategory = await _context.ProductCategories.Where(x => x.Name.ToLower() == category.Name.ToLower()).FirstOrDefaultAsync();
            if (mycategory != null) return false;

            _context.ProductCategories.Update(category);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> UpdateProductType(ProductType type)
        {
            var myType = await _context.ProductTypes.Where(x => x.Name.ToLower() == type.Name.ToLower()).FirstOrDefaultAsync();
            if (myType != null) return false;

            _context.ProductTypes.Update(type);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> AddEntityList(IReadOnlyList<T> entities)
        {
            await _context.AddRangeAsync(entities);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> RemoveEntity(int id)
        {
            var entity = await _context.Set<T>().FindAsync(id);
            _context.Remove(entity);
            return await _context.SaveChangesAsync() > 0;
            
        }

        public async Task<bool> UpdateEntity(T entity)
        {
            _context.Update(entity);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<IReadOnlyList<Product>> GetProductsAsync(ProductSpecParams pageParams)
        {
            var result = await _context.Products.Include(p => p.ProductType).Include(p => p.ProductCategory).Include(p => p.Promotion).Include(p => p.PriceHistory).Include(x => x.Supplier).Where(x => (string.IsNullOrEmpty(pageParams.Search) || x.Name.ToLower().Contains(pageParams.Search.ToLower())) &&
                                                (!pageParams.SupplierId.HasValue || x.SupplierId == pageParams.SupplierId) &&
                                                (!pageParams.CategoryId.HasValue || x.ProductCategoryId == pageParams.CategoryId) &&
                                                (!pageParams.TypeId.HasValue || x.ProductTypeId == pageParams.TypeId)).ToListAsync();
            
                switch (pageParams.sort)
                {
                case "priceAsc":
                    result = result.OrderBy(p => p.GetPrice()).ToList();
                    break;
                case "priceDesc":
                    result = result.OrderByDescending(p => p.GetPrice()).ToList();
                    break;
                case "quantityAsc":
                    result = result.OrderBy(p => p.Quantity).ToList();
                    break;
                case "quantityDesc":
                    result = result.OrderByDescending(p => p.Quantity).ToList();
                    break;
                case "thresholdAsc":
                    result = result.OrderBy(p => p.TresholdValue).ToList();
                    break;
                case "thresholdDesc":
                    result = result.OrderByDescending(p => p.TresholdValue).ToList();
                    break;
                default:
                    result = result.OrderBy(n => n.Name).ToList();
                        break;
                }

            return result;
        }

        public async Task<Product> GetProductByIdAsync(int id)
        {
            return await _context.Products.Where(p => p.Id == id).Include(x => x.Supplier).Include(x => x.Promotion).Include(p => p.ProductType).Include(p => p.ProductCategory).Include(x=> x.PriceHistory).FirstOrDefaultAsync();
        }

        public async Task<IReadOnlyList<FAQ>> GetFaqsAsync(SpecParams specParams)
        {
            var faqs = await _context.FAQs.Where(x => string.IsNullOrEmpty(specParams.Search) || x.Question.ToLower().Contains(specParams.Search.ToLower()) || x.Answer.ToLower().Contains(specParams.Search.ToLower())).ToListAsync();

            switch (specParams.sort)
            {
                case "AnsAsc":
                    faqs = faqs.OrderBy(x => x.Answer).ToList();
                    break;
                case "DescAns":
                    faqs = faqs.OrderByDescending(x => x.Answer).ToList();
                    break;
                default:
                    faqs = faqs.OrderBy(x => x.Answer).ToList();
                    break;
            }

            return faqs;
        }

        public async Task<IReadOnlyList<Supplier>> GetSuppliersAsync(SpecParams specParams)
        {
            var suppliers = await _context.Suppliers.Where(x => string.IsNullOrEmpty(specParams.Search) || x.Name.ToLower().Contains(specParams.Search.ToLower())).ToListAsync();

            switch (specParams.sort)
            {
                case "nameAsc":
                    suppliers = suppliers.OrderBy(x => x.Name).ToList();
                    break;
                case "nameDesc":
                    suppliers = suppliers.OrderByDescending(x => x.Name).ToList();
                    break;
                default:
                    suppliers = suppliers.OrderBy(x => x.Name).ToList();
                    break;
            }
            return suppliers;
        }

        public async Task<IReadOnlyList<GiftBox>> GetGiftBoxesAsync(SpecParams specParams)
        {
            var giftboxes = await _context.GiftBoxes.Include(x => x.PriceHistory).Where(x => (string.IsNullOrEmpty(specParams.Search) || x.Name.ToLower().Contains(specParams.Search.ToLower()) || x.Description.ToLower().Contains(specParams.Search.ToLower()))).ToListAsync();

            switch (specParams.sort)
            {
                case "nameAsc":
                    giftboxes = giftboxes.OrderBy(x => x.Name).ToList();
                    break;
                case "nameDesc":
                    giftboxes = giftboxes.OrderByDescending(x => x.Name).ToList();
                    break;
                case "priceAsc":
                    giftboxes = giftboxes.OrderBy(x => x.PriceHistory).ToList();
                    break;
                case "priceDesc":
                    giftboxes = giftboxes.OrderByDescending(x => x.PriceHistory).ToList();
                    break;
                default:
                    giftboxes = giftboxes.OrderBy(x => x.Name).ToList();
                    break;
            }

            return giftboxes;
        }

        public async Task<GiftBox> GetGiftBoxByIdAsync(int id)
        {
            var result = await _context.GiftBoxes.Where(p => p.Id == id).Include(x => x.Products).ThenInclude(y => y.ProductCategory).Include(x => x.Products).ThenInclude(y => y.PriceHistory).Include(x => x.Products).ThenInclude(y => y.ProductType).Include(x => x.PriceHistory).FirstOrDefaultAsync();

            return result;
        }

        public async Task<IReadOnlyList<Vat>> GetVatsAsync(VatSpecParams specParams)
        {
            var vats = await _context.Vats.Where(x => !specParams.IsActive.HasValue || x.IsActive == specParams.IsActive).ToListAsync();

            switch (specParams.sort)
            {
                case "startDateAsc":
                    vats = vats.OrderBy(x => x.StartDate).ToList();
                    break;
                case "startDateDesc":
                    vats = vats.OrderByDescending(x => x.StartDate).ToList();
                    break;
                case "endDateAsc":
                    vats = vats.OrderBy(x => x.EndDate).ToList();
                    break;
                case "endDateDesc":
                    vats = vats.OrderByDescending(x => x.EndDate).ToList();
                    break;
                default:
                    vats = vats.OrderByDescending(x => x.StartDate).ToList();
                    break;
            }
            return vats;
        }

        public async Task<Vat> GetActiveVat()
        {
            return await _context.Vats.Where(x => x.IsActive == true).FirstOrDefaultAsync();
        }

        public async Task<IReadOnlyList<Promotion>> GetPromotionsAsync(PromotionSpecParams specParams)
        {
            var promotions = await _context.Promotions.Where(x => string.IsNullOrEmpty(specParams.Search) || x.Name.ToLower().Contains(specParams.Search.ToLower()) || x.Description.ToLower().Contains(specParams.Search.ToLower()) &&
            (!specParams.IsActive.HasValue || x.IsActive == specParams.IsActive)).ToListAsync();

            switch (specParams.sort)
            {
                case "nameAsc":
                    promotions = promotions.OrderBy(x => x.Name).ToList();
                    break;
                case "nameDesc":
                    promotions = promotions.OrderByDescending(x => x.Name).ToList();
                    break;
                case "percentageAsc":
                    promotions = promotions.OrderBy(x => x.Percentage).ToList();
                    break;
                case "percentageDesc":
                    promotions = promotions.OrderByDescending(x => x.Percentage).ToList();
                    break;
                default:
                    promotions = promotions.OrderBy(x => x.Name).ToList();
                    break;
            }

            return promotions;
        }

        public async Task<Promotion> GetPromotionByIdAsync(int id)
        {
            var promotions = await _context.Promotions.Where(x => x.Id == id).Include(x => x.Products).ThenInclude(y => y.ProductCategory).Include(x => x.Products).ThenInclude(y => y.ProductType).FirstOrDefaultAsync();
            promotions.Products = await _context.Products.Where(x => x.PromotionId == promotions.Id).Include(y => y.ProductCategory).Include(y => y.ProductType).ToListAsync();
            return promotions;
        }

        public async Task<IReadOnlyList<Order>> GetUserOrdersAsync(string customerEmail, OrderParams searchParams)
        {
            var orders = await _context.Orders
                .Include(x => x.ShipToAddress)
                .Include(x => x.OrderItems)
                .Include(x => x.DeliveryMethod)
                .Include(x => x.OrderType)
                .Where(x => (x.CustomerEmail == customerEmail) &&
                            (string.IsNullOrEmpty(searchParams.Search) || x.CustomerEmail.ToLower().Contains(searchParams.Search.ToLower())) &&
                            (!searchParams.DeliveryMethodId.HasValue || x.DeliveryMethodId == searchParams.DeliveryMethodId))
                .ToListAsync();


            switch (searchParams.sort)
            {
                case "dateAsc":
                    orders = orders.OrderBy(x => x.OrderDate).ToList();
                    break;
                case "dateDesc":
                    orders = orders.OrderByDescending(x => x.OrderDate).ToList();
                    break;
                case "totalAsc":
                    orders = orders.OrderBy(x => x.SubTotal).ToList();
                    break;
                case "totalDesc":
                    orders = orders.OrderByDescending(x => x.SubTotal).ToList();
                    break;
                default:
                    orders = orders.OrderByDescending(x => x.OrderDate).ToList();
                    break;
            }
            return orders;
        }

        public async Task<Order> GetOrderByIdAsync(int id)
        {
             return await _context.Orders.Where(x => x.Id == id)
            .Include(x => x.ShipToAddress)
            .Include(x => x.OrderItems)
            .Include(x => x.DeliveryMethod)
            .Include(x => x.OrderType)
            .FirstOrDefaultAsync();
        }

        public async Task<IReadOnlyList<DeliveryMethod>> GetDeliveryMethodsAsync()
        {
            return await _context.DeliveryMethods.ToListAsync();
        }

        public async Task<bool> VerifyStockForSaleOrder(string cartId)
        {
            var cart = await _basketRepository.GetBasketAsync(cartId);
            foreach (var item in cart.Items)
            {
                var productItem = await this.GetProductByIdAsync(item.Id);
                if(productItem.Quantity < item.Quantity)
                {
                    return false;
                }
            }
            return true;
        }

        public async Task<Order> CreateOrderAsync(string customerRmail, int deliveryMethodId, int orderTypeId, string cartId, OrderAddress shippingAddress)
        {
            var cart = await _basketRepository.GetBasketAsync(cartId);

            var items = new List<OrderItem>();
            foreach (var item in cart.Items)
            {
                var productItem = await this.GetProductByIdAsync(item.Id);
                var productordered = new ProductOrdered(productItem.Id, productItem.Name, productItem.PictureUrl);
                var orderItem = new OrderItem(productordered, productItem.PriceHistory.OrderByDescending(pp => pp.StartDate).First().Price, item.Quantity);
                if (productItem.PromotionId != null && productItem.Promotion.IsActive != false)
                {
                    orderItem.Promotion = productItem.Promotion.Percentage;
                    productordered.PromotionId = productItem.PromotionId;
                }
                items.Add(orderItem);

                productItem.Quantity -= item.Quantity;
                _context.Products.Update(productItem);
            }

            var deliveryMethod = await _context.DeliveryMethods.Where(x => x.Id == deliveryMethodId).FirstOrDefaultAsync();

            decimal subtotal = 0;
            foreach (var item in items)
            {
                if (item.Promotion != 0)
                {
                    subtotal += (item.Price * item.Quantity) * (1 - item.Promotion / 100);
                }
                else
                {
                    subtotal += (item.Price * item.Quantity);
                }
            }

            var order = new Order(items, customerRmail, shippingAddress, deliveryMethod, orderTypeId, subtotal);

            order.OrderType = await _context.OrderTypes.Where(x => x.Id == orderTypeId).FirstOrDefaultAsync();

            order.Status = OrderStatus.PaymentConfirmed;

            // Applying Group Elephant discount
            var user = await _userManager.FindByEmailAsync(customerRmail);
            var result = await _userManager.IsInRoleAsync(user, "GroupElephantcompany");
            if (result == true)
            {
                order.GroupElephantDiscount = 10;
            }

            this.GetActiveVat();


            await _context.Orders.AddAsync(order);

            await _context.SaveChangesAsync();

            foreach(var item in items)
            {
                var product = await this.GetProductByIdAsync(item.Id);
                if (product.Quantity <= product.TresholdValue)
                {
                    var result4 = CreateAutomatedSupplierOrderAsync("mzamotembe7@gmail.com", product);
                }
            }

            return order;
        }

        public async Task<IReadOnlyList<Order>> GetOrdersAsync(OrderParams searchParams)
        {
            var orders = await _context.Orders
                .Include(x => x.ShipToAddress)
                .Include(x => x.OrderItems)
                .Include(x => x.DeliveryMethod)
                .Include(x => x.OrderType)
                .Where(x => (string.IsNullOrEmpty(searchParams.Search) || x.CustomerEmail.ToLower().Contains(searchParams.Search.ToLower())) &&
                            (!searchParams.DeliveryMethodId.HasValue || x.DeliveryMethodId == searchParams.DeliveryMethodId))
                .ToListAsync();

            switch (searchParams.sort)
            {
                case "dateAsc":
                    orders = orders.OrderBy(x => x.OrderDate).ToList();
                    break;
                case "dateDesc":
                    orders = orders.OrderByDescending(x => x.OrderDate).ToList();
                    break;
                case "totalAsc":
                    orders = orders.OrderBy(x => x.SubTotal).ToList();
                    break;
                case "totalDesc":
                    orders = orders.OrderByDescending(x => x.SubTotal).ToList();
                    break;
                default:
                    orders = orders.OrderByDescending(x => x.OrderDate).ToList();
                    break;
            }
            return orders;
        }

        public async Task<IReadOnlyList<WriteOff>> GetWriteOffsAsync(WriteOffParams writeoffParams)
        {
            var orders = await _context.WriteOffs
                .Include(x => x.Product)
                .Where(x =>!writeoffParams.ProductId.HasValue || x.ProductId == writeoffParams.ProductId)
                .ToListAsync();


            switch (writeoffParams.sort)
            {
                case "dateAsc":
                    orders = orders.OrderBy(x => x.Date).ToList();
                    break;
                case "dateDesc":
                    orders = orders.OrderByDescending(x => x.Date).ToList();
                    break;
                case "quantityAsc":
                    orders = orders.OrderBy(x => x.Quantity).ToList();
                    break;
                case "quantityDesc":
                    orders = orders.OrderByDescending(x => x.Quantity).ToList();
                    break;
                default:
                    orders = orders.OrderByDescending(x => x.Date).ToList();
                    break;
            }
            return orders;
        }

        public async Task<WriteOff> GetWriteOffByIdAsync(int id)
        {
            return await _context.WriteOffs.Where(x => x.Id == id).Include(x => x.Product).FirstOrDefaultAsync();
        }

        public async Task<bool> CreateWriteOff(WriteOff wrtieoff)
        {
            var product = await _context.Products.Where(x => x.Id == wrtieoff.ProductId).Include(x => x.Supplier).Include(x => x.PriceHistory).FirstOrDefaultAsync();
            if (product.Quantity < wrtieoff.Quantity) return false;
            product.Quantity = product.Quantity - wrtieoff.Quantity;
            wrtieoff.ProductPrice = product.GetPrice();

            var result1 = await _context.WriteOffs.AddAsync(wrtieoff);
            var result2 = _context.Products.Update(product);
            var result3 = await _context.SaveChangesAsync() > 0;

            if (product.Quantity <= product.TresholdValue)
            {
                var result4 = CreateAutomatedSupplierOrderAsync("mzamotembe7@gmail.com", product);
            }
            return result3;
        }

        public async Task<CustomerReturn> LogCustomerReturnAsync(int orderId, string customerEmail, string ReturnDescription, List<ReturnItem> returnedItems)
        {
            var ReturnItems = new List<ReturnItem>();
            foreach (var item in returnedItems)
            {
                var product = await _context.Products.Where(x => x.Id == item.ProductId).Include(x => x.PriceHistory).FirstOrDefaultAsync();
                ReturnItems.Add(new ReturnItem
                {
                    ProductId = product.Id,
                    ProductName = product.Name,
                    PictureUrl = product.PictureUrl,
                    Quantity = item.Quantity,
                    Price = product.PriceHistory.OrderByDescending(pp => pp.StartDate).First().Price
                });
                product.Quantity += item.Quantity;
                _context.Products.Update(product);
            }
            var total = ReturnItems.Sum(item => item.Price * item.Quantity);

            var custReturn = new CustomerReturn(orderId, customerEmail, ReturnDescription, DateTime.Now, total, ReturnItems);

            await _context.CustomerReturns.AddAsync(custReturn);
            await _context.SaveChangesAsync();

            return custReturn;
        }

        public async Task<bool> VerifyReturnRequest(int orderId, string customerEmail, List<ReturnItem> returnedItems)
        {
            var user = await _userManager.FindByEmailAsync(customerEmail);
            if (user == null)
            {
                return false;
            }

            var order = await _context.Orders.Where(x => x.Id == orderId).Include(x => x.OrderItems).ThenInclude(x => x.ItemOrdered).FirstOrDefaultAsync();
            if (order == null || order.Status != OrderStatus.Dispatched)
            {
                return false;
            }

            if (returnedItems.GroupBy(x => x.ProductId).Where(g => g.Count() > 1).SelectMany(g => g).Count() > 1)
            {
                return false;
            }

            foreach (var item in returnedItems)
            {
                if(order.OrderItems.Where(x => x.ItemOrdered.ProductItemId == item.ProductId).FirstOrDefault() == null || order.OrderItems.Where(x => x.ItemOrdered.ProductItemId == item.ProductId).FirstOrDefault().Quantity < item.Quantity)
                {
                    return false;
                }

                var customerReturnItems = await _context.CustomerReturns.Where(x => x.OrderId == orderId).Include(x => x.ReturnItems).FirstOrDefaultAsync();
                if (customerReturnItems != null)
                {
                    var previousCustomerReturn = customerReturnItems.ReturnItems.Where(x => x.ProductId == item.ProductId).FirstOrDefault();
                    if (previousCustomerReturn != null)
                    {
                        return false;
                    }
                }
            }
            return true;
        }

        public async Task<IReadOnlyList<CustomerReturn>> GetReturnsLogAsync(SpecParams searchParams)
        {
            var returnLog = await _context.CustomerReturns
                .Include(x => x.ReturnItems)
                .Where(x => (string.IsNullOrEmpty(searchParams.Search) || x.CustomerEmail.ToLower().Contains(searchParams.Search.ToLower()) || x.Description.ToLower().Contains(searchParams.Search.ToLower()) ))
                .ToListAsync();

            switch (searchParams.sort)
            {
                case "dateAsc":
                    returnLog = returnLog.OrderBy(x => x.Date).ToList();
                    break;
                case "dateDesc":
                    returnLog = returnLog.OrderByDescending(x => x.Date).ToList();
                    break;
                case "totalAsc":
                    returnLog = returnLog.OrderBy(x => x.Total).ToList();
                    break;
                case "totalDesc":
                    returnLog = returnLog.OrderByDescending(x => x.Total).ToList();
                    break;
                default:
                    returnLog = returnLog.OrderByDescending(x => x.Date).ToList();
                    break;
            }
            return returnLog;
        }

        public async Task<CustomerReturn> GetCustomerReturnByIdAsync(int id)
        {
            return await _context.CustomerReturns.Where(x => x.Id == id).Include(x => x.ReturnItems).FirstOrDefaultAsync();
        }

        public async Task<IReadOnlyList<OrderType>> GetOrderTypesAsync()
        {
            return await _context.OrderTypes.ToListAsync();
        }

        public async Task<bool> DispatchOrderAsync(int orderId, string trackingNumber)
        {
            var order = await _context.Orders.Where(x => x.Id == orderId).Include(x => x.ShipToAddress).Include(x => x.OrderType).FirstOrDefaultAsync();
            if(order == null)
            {
                return false;
            }

            if(order.OrderType.Name == "Delivery" && order.ShipToAddress.TrackingNumber == null)
            {
                order.ShipToAddress.TrackingNumber = trackingNumber;
            }
            order.Status = OrderStatus.Dispatched;

            _context.Orders.Update(order);
            return await _context.SaveChangesAsync() > 0;

        }

        public async Task<bool> PackageOrder(int orderId)
        {
            var order = await _context.Orders.Where(x => x.Id == orderId).Include(x => x.OrderType).FirstOrDefaultAsync();
            if (order == null)
            {
                return false;
            }

            if (order.OrderType.Name == "Collection" && order.Status != OrderStatus.ReadyForCollection)
            {
                order.Status = OrderStatus.ReadyForCollection;
            }
            _context.Orders.Update(order);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<IReadOnlyList<ProductReview>> GetProductReviewsAsync(ReviewParams searchParams)
        {
            var reviews = await _context.ProductReviews
                .Include(x => x.Product)
                .Where(x => (string.IsNullOrEmpty(searchParams.Search) || 
                    x.CustomerEmail.ToLower().Contains(searchParams.Search.ToLower()) || 
                    x.Title.ToLower().Contains(searchParams.Search.ToLower()) ||
                    x.Description.ToLower().Contains(searchParams.Search.ToLower())) &&
                    (!searchParams.ProductId.HasValue || x.ProductId == searchParams.ProductId))
                .ToListAsync();

            switch (searchParams.sort)
            {
                case "dateAsc":
                    reviews = reviews.OrderBy(x => x.Date).ToList();
                    break;
                case "dateDesc":
                    reviews = reviews.OrderByDescending(x => x.Date).ToList();
                    break;
                case "ratingAsc":
                    reviews = reviews.OrderBy(x => x.Rating).ToList();
                    break;
                case "ratingDesc":
                    reviews = reviews.OrderByDescending(x => x.Rating).ToList();
                    break;
                default:
                    reviews = reviews.OrderByDescending(x => x.Date).ToList();
                    break;
            }
            return reviews;
        }

        public Task<ProductReview> GetProductReviewByIdAsync(int id)
        {
            return _context.ProductReviews.Where(x => x.Id == id).Include(x => x.Product).FirstOrDefaultAsync();
        }

        public async Task<bool> CreateProductReviewAsync(ProductReview prodReview)
        {
            await _context.ProductReviews.AddAsync(prodReview);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> FlagProductReviewAsync(int id)
        {
            var review = await GetProductReviewByIdAsync(id);
            review.Status = ProductReviewStatus.Flagged;

             _context.ProductReviews.Update(review);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> UnFlagProductReviewAsync(int id, string ManagerEmail)
        {
            var manager = await _userManager.FindByEmailAsync(ManagerEmail);
            if (manager == null) return false;

            var result = await _userManager.IsInRoleAsync(manager, "Manager");
            if (result == false) return false;

            var review = await GetProductReviewByIdAsync(id);
            review.Status = ProductReviewStatus.Available;

            _context.ProductReviews.Update(review);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<IReadOnlyList<SupplierOrder>> GetSupplierOrdersAsync(OrderParams searchParams)
        {
            var orders = await _context.SupplierOrders
                .Include(x => x.OrderItems)
                .Include(x => x.CompanyDetails)
                .Include(x => x.Supplier)
                .Where(x => (string.IsNullOrEmpty(searchParams.Search) || x.ManagerEmail.ToLower().Contains(searchParams.Search.ToLower())))
                .ToListAsync();

            switch (searchParams.sort)
            {
                case "dateAsc":
                    orders = orders.OrderBy(x => x.OrderDate).ToList();
                    break;
                case "dateDesc":
                    orders = orders.OrderByDescending(x => x.OrderDate).ToList();
                    break;
                case "totalAsc":
                    orders = orders.OrderBy(x => x.Total).ToList();
                    break;
                case "totalDesc":
                    orders = orders.OrderByDescending(x => x.Total).ToList();
                    break;
                default:
                    orders = orders.OrderByDescending(x => x.OrderDate).ToList();
                    break;
            }
            return orders;
        }

        public async Task<SupplierOrder> GetSupplierOrderByIdAsync(int id)
        {
            return await _context.SupplierOrders
                                .Include(x => x.OrderItems)
                                .Include(x => x.CompanyDetails)
                                .Include(x => x.Supplier)
                                .Where(x => x.Id == id).FirstOrDefaultAsync();
        }

        public async Task<bool> CreateAutomatedSupplierOrderAsync(string managerEmail, Product product)
        {
            int quantity = product.TresholdValue - product.Quantity + 1;
            decimal total = quantity * product.PriceHistory.OrderByDescending(pp => pp.StartDate).First().Price;

            var items = new List<SupplierOrderItem>();
            var productordered = new SupplierProductOrdered(product.Id, product.Name, product.PictureUrl);
            var orderItem = new SupplierOrderItem(productordered, product.PriceHistory.OrderByDescending(pp => pp.StartDate).First().Price, quantity);
            items.Add(orderItem);

            var company = await _context.Company.FirstOrDefaultAsync();
            var order = new SupplierOrder(items, managerEmail, company, product.Supplier, total);
            order.Status = SupplierOrderStatus.Pending;

            await _context.SupplierOrders.AddAsync(order);

            EmailVM request_customer = new EmailVM
            {
                To = "mzamotembe7@gmail.com",
                Subject = "Low Stock",
                Body = "THe product " + product.Name + " has reach its low stock level. Please buy more stock before the quantity reaches 0."
            };
            _emailservice.SendEmail(request_customer);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<SupplierOrder> CreateSupplierOrderAsync(string managerEmail, int supplierId, List<SupplierOrderItemVM> orderItems)
        {
            var items = new List<SupplierOrderItem>();
            foreach (var item in orderItems)
            {
                var productItem = await this.GetProductByIdAsync(item.ProductId);
                if(productItem.SupplierId == supplierId)
                {
                    var productordered = new SupplierProductOrdered(productItem.Id, productItem.Name, productItem.PictureUrl);
                    var orderItem = new SupplierOrderItem(productordered, productItem.PriceHistory.OrderByDescending(pp => pp.StartDate).First().Price, item.Quantity);
                    items.Add(orderItem);
                }
            }

            decimal total = 0;
            foreach (var item in items)
            {
                total += (item.Price * item.Quantity);
            }

            var company = await _context.Company.FirstOrDefaultAsync();
            var supplier = await _context.Suppliers.Where(x => x.Id == supplierId).FirstOrDefaultAsync();

            var order = new SupplierOrder(items, managerEmail, company, supplier, total);
            order.Status = SupplierOrderStatus.EmailSent;
            await _context.SupplierOrders.AddAsync(order);
            await _context.SaveChangesAsync();

            StringBuilder body = new StringBuilder();
            foreach(var item in orderItems)
            {
                body.AppendLine("- " +  item.ProductName.ToString() + ", Quantity = " + item.Quantity.ToString());
            }
            _emailservice.SendEmail(new EmailVM
            {
                To = supplier.Email,
                Subject = "Supplier Order - ERP",
                Body = "Good day, I would like to make a supplier order the following items. " + body.ToString(),
            });
            return order;
        }

        public async Task<bool> ApproveSupplierOrder(int orderId, List<SupplierOrderItemVM> orderItems)
        {
            var order = await _context.SupplierOrders.Where(x => x.Id == orderId).Include(x => x.OrderItems).FirstOrDefaultAsync();
            if (order == null) return false;
            decimal total = 0;

            foreach(var item in order.OrderItems)
            {
                    _context.SupplierOrderItems.Remove(item);
            }
            order.OrderItems.Clear();

            foreach (var item in orderItems)
            {
                var productItem = await this.GetProductByIdAsync(item.ProductId);
                if (productItem.SupplierId == order.SupplierId)
                {
                    var productordered = new SupplierProductOrdered(productItem.Id, productItem.Name, productItem.PictureUrl);
                    var orderItem = new SupplierOrderItem(productordered, productItem.PriceHistory.OrderByDescending(pp => pp.StartDate).First().Price, item.Quantity);
                    order.OrderItems.Add(orderItem);
                    total += orderItem.Price * orderItem.Quantity;
                }
            }
            order.Total = total;
            order.Status = SupplierOrderStatus.EmailSent;
            _context.SupplierOrders.Update(order);
            return await _context.SaveChangesAsync() > 0;
        }


        public async Task<bool> ConfirmSupplierOrderPayment(int orderId, string InvoiceUrl, string ProofOfPaymentUrl)
        {
            var order = _context.SupplierOrders.Where(x => x.Id == orderId).FirstOrDefault();
            if (order == null) return false;

            order.InvoiceUrl = InvoiceUrl;
            order.ProofOfPaymentUrl = ProofOfPaymentUrl;
            order.Status = SupplierOrderStatus.PaymentConfirmed;
            _context.SupplierOrders.Update(order);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> ConfirmSupplierOrderDelivery(int orderId, List<SupplierOrderItemVM> orderItems)
        {
            var order = _context.SupplierOrders.Where(x => x.Id == orderId).Include(x => x.OrderItems).FirstOrDefault();
            order.TotalNotDelivered = 0;
            if (order == null) return false;

            foreach (var item in orderItems)
            {
                var result = order.OrderItems.Where(x => x.ItemOrdered.ProductItemId == item.ProductId).FirstOrDefault();
                // The supplier has under delivered
                if (result != null && result.Quantity > item.Quantity)
                {
                    result.QuantityNotDelivered = result.Quantity - item.Quantity;
                    order.TotalNotDelivered += result.QuantityNotDelivered * result.Price;
                }
                if(result != null && result.Quantity < item.Quantity)
                {
                    return false;
                }
                var productItem = await this.GetProductByIdAsync(item.ProductId);
                productItem.Quantity += item.Quantity;
                _context.Products.Update(productItem);
            }
            order.Status = SupplierOrderStatus.OrderRecieved;
            _context.SupplierOrders.Update(order);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> CancelSupplierOrder(int orderId)
        {
            var order = _context.SupplierOrders.Where(x => x.Id == orderId).FirstOrDefault();
            if(order == null) return false;

            order.Status = SupplierOrderStatus.Cancelled;
            _context.SupplierOrders.Update(order);

            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<SupplierReturn> LogSupplierReturnAsync(int orderId, string managerEmail, string ReturnDescription, List<SupplierReturnItem> returnedItems)
        {
            var ReturnItems = new List<SupplierReturnItem>();
            foreach (var item in returnedItems)
            {
                var product = await _context.Products.Where(x => x.Id == item.ProductId).Include(x => x.PriceHistory).FirstOrDefaultAsync();
                ReturnItems.Add(new SupplierReturnItem
                {
                    ProductId = product.Id,
                    ProductName = product.Name,
                    PictureUrl = product.PictureUrl,
                    Quantity = item.Quantity,
                    Price = product.PriceHistory.OrderByDescending(pp => pp.StartDate).First().Price
                });
                product.Quantity -= item.Quantity;
                _context.Products.Update(product);
            }
            var total = ReturnItems.Sum(item => item.Price * item.Quantity);

            var custReturn = new SupplierReturn(orderId, managerEmail, ReturnDescription, DateTime.Now, total, ReturnItems);

            await _context.SupplierReturns.AddAsync(custReturn);
            await _context.SaveChangesAsync();

            foreach (var item in ReturnItems)
            {
                var product = await this.GetProductByIdAsync(item.ProductId);
                if (product.Quantity <= product.TresholdValue)
                {
                    var result4 = CreateAutomatedSupplierOrderAsync("mzamotembe7@gmail.com", product);
                }
            }

            return custReturn;
        }

        public async Task<bool> VerifySupplierReturnRequest(int orderId, List<SupplierReturnItem> returnedItems)
        {
            var order = await _context.SupplierOrders.Where(x => x.Id == orderId).Include(x => x.OrderItems).ThenInclude(x => x.ItemOrdered).FirstOrDefaultAsync();
            if (order == null || order.Status != SupplierOrderStatus.OrderRecieved)
            {
                return false;
            }

            if (returnedItems.GroupBy(x => x.ProductId).Where(g => g.Count() > 1).SelectMany(g => g).Count() > 1)
            {
                return false;
            }

            foreach (var item in returnedItems)
            {
                var product = await _context.Products.Where(x => x.Id == item.ProductId).FirstOrDefaultAsync();
                if(product.Quantity < item.Quantity)
                {
                    return false;
                }

                if (order.OrderItems.Where(x => x.ItemOrdered.ProductItemId == item.ProductId).FirstOrDefault() == null || 
                    (order.OrderItems.Where(x => x.ItemOrdered.ProductItemId == item.ProductId).FirstOrDefault().Quantity -
                    order.OrderItems.Where(x => x.ItemOrdered.ProductItemId == item.ProductId).FirstOrDefault().QuantityNotDelivered)
                    < item.Quantity)
                {
                    return false;
                }

                var supplierReturnItems = await _context.SupplierReturns.Where(x => x.SupplierOrderId == orderId).Include(x => x.ReturnItems).FirstOrDefaultAsync();
                if (supplierReturnItems != null)
                {
                    var previousSupplierReturn = supplierReturnItems.ReturnItems.Where(x => x.ProductId == item.ProductId).FirstOrDefault();
                    if (previousSupplierReturn != null)
                    {
                        return false;
                    }
                }
            }
            return true;
        }

        public async Task<IReadOnlyList<SupplierReturn>> GetSupplierReturnsLogAsync(SpecParams searchParams)
        {
            var returnLog = await _context.SupplierReturns
                .Include(x => x.ReturnItems)
                .Where(x => (string.IsNullOrEmpty(searchParams.Search) || x.ManagerEmail.ToLower().Contains(searchParams.Search.ToLower()) || x.Description.ToLower().Contains(searchParams.Search.ToLower())))
                .ToListAsync();

            switch (searchParams.sort)
            {
                case "dateAsc":
                    returnLog = returnLog.OrderBy(x => x.Date).ToList();
                    break;
                case "dateDesc":
                    returnLog = returnLog.OrderByDescending(x => x.Date).ToList();
                    break;
                case "totalAsc":
                    returnLog = returnLog.OrderBy(x => x.Total).ToList();
                    break;
                case "totalDesc":
                    returnLog = returnLog.OrderByDescending(x => x.Total).ToList();
                    break;
                default:
                    returnLog = returnLog.OrderByDescending(x => x.Date).ToList();
                    break;
            }
            return returnLog;
        }

        public async Task<SupplierReturn> GetSupplierReturnByIdAsync(int id)
        {
            return await _context.SupplierReturns.Where(x => x.Id == id).Include(x => x.ReturnItems).FirstOrDefaultAsync();
        }

        public async Task<IReadOnlyList<DeliveryMethod>> GeDeliveryMethodsAsync(SpecParams searchParams)
        {
            var deliveryMethods = await _context.DeliveryMethods
                .Where(x => (string.IsNullOrEmpty(searchParams.Search) || x.Name.ToLower().Contains(searchParams.Search.ToLower()) || x.Description.ToLower().Contains(searchParams.Search.ToLower()) || x.DeliveryTime.ToLower().Contains(searchParams.Search.ToLower())))
                .ToListAsync();

            switch (searchParams.sort)
            {
                case "priceAsc":
                    deliveryMethods = deliveryMethods.OrderBy(x => x.Price).ToList();
                    break;
                case "priceDesc":
                    deliveryMethods = deliveryMethods.OrderByDescending(x => x.Price).ToList();
                    break;
                case "nameAsc":
                    deliveryMethods = deliveryMethods.OrderBy(x => x.Name).ToList();
                    break;
                case "nameDesc":
                    deliveryMethods = deliveryMethods.OrderByDescending(x => x.Name).ToList();
                    break;
                default:
                    deliveryMethods = deliveryMethods.OrderByDescending(x => x.Name).ToList();
                    break;
            }
            return deliveryMethods;
        }

        public async Task<IReadOnlyList<Media>> GetMediaListAsync(SpecParams searchParams)
        {
            var MediaList = await _context.MediaItems
                .Where(x => (string.IsNullOrEmpty(searchParams.Search) || x.Name.ToLower().Contains(searchParams.Search.ToLower()) || x.Description.ToLower().Contains(searchParams.Search.ToLower())))
                .ToListAsync();

            switch (searchParams.sort)
            {
                case "nameAsc":
                    MediaList = MediaList.OrderBy(x => x.Name).ToList();
                    break;
                case "nameDesc":
                    MediaList = MediaList.OrderByDescending(x => x.Name).ToList();
                    break;
                default:
                    MediaList = MediaList.OrderByDescending(x => x.Name).ToList();
                    break;
            }
            return MediaList;
        }


        public async Task<bool> RemoveProduct(int id)
        {
            var entity = await _context.Products.FindAsync(id);

            var isInPromotion = await _context.Promotions.Where(x => x.Products.Contains(entity)).FirstOrDefaultAsync();
            var isInGiftBox = await _context.GiftBoxes.Where(x => x.Products.Contains(entity)).FirstOrDefaultAsync();
            var isInSaleOrder = await _context.OrderItems.Where(x => x.ItemOrdered.ProductItemId == id).FirstOrDefaultAsync();
            var isInSupplierOrder = await _context.SupplierOrderItems.Where(x => x.ItemOrdered.ProductItemId == id).FirstOrDefaultAsync();
            var isInSaleReturn = await _context.ReturnItems.Where(x => x.ProductId == id).FirstOrDefaultAsync();
            var isInSupplierReturn = await _context.SupplierReturnItems.Where(x => x.ProductId == id).FirstOrDefaultAsync();
            var isInWriteOff = await _context.WriteOffs.Where(x => x.ProductId == id).FirstOrDefaultAsync();
            var isInProductReview = await _context.ProductReviews.Where(x => x.ProductId == id).FirstOrDefaultAsync();

            if (isInPromotion != null || isInGiftBox != null || isInSaleOrder != null 
                || isInSupplierOrder != null || isInSaleReturn != null 
                || isInSupplierReturn != null || isInWriteOff != null || isInProductReview != null)
            {
                return false;
            }
            else
            {
                _context.Remove(entity);
                return await _context.SaveChangesAsync() > 0;
            }
        }

        public async Task<bool> RemoveProductType(int id)
        {
            var entity = await _context.ProductTypes.FindAsync(id);
            var isInProduct = await _context.Products.Where(x => x.ProductTypeId == id).FirstOrDefaultAsync();  
            if (isInProduct != null)
            {
                return false;
            }
            else
            {
                _context.ProductTypes.Remove(entity);
                return await _context.SaveChangesAsync() > 0;
            }
        }

        public async Task<bool> RemoveProductCategory(int id)
        {
            var entity = await _context.ProductCategories.FindAsync(id);
            var isInProduct = await _context.Products.Where(x => x.ProductCategoryId == id).FirstOrDefaultAsync();
            if (isInProduct != null)
            {
                return false;
            }
            else
            {
                _context.ProductCategories.Remove(entity);
                return await _context.SaveChangesAsync() > 0;
            }
        }

        public async Task<bool> RemoveSupplier(int id)
        {
            var entity = await _context.Suppliers.FindAsync(id);
            var isInProduct = await _context.Products.Where(x => x.SupplierId == id).FirstOrDefaultAsync();
            if (isInProduct != null)
            {
                return false;
            }
            else
            {
                _context.Suppliers.Remove(entity);
                return await _context.SaveChangesAsync() > 0;
            }
        }

        public async Task<bool> RemoveGiftBox(int id)
        {
            var entity = await _context.GiftBoxes.FindAsync(id);
            var isInProduct = await _context.Products.Where(x => x.GiftBoxes.Contains(entity)).FirstOrDefaultAsync();
            if (isInProduct != null)
            {
                return false;
            }
            else
            {
                _context.GiftBoxes.Remove(entity);
                return await _context.SaveChangesAsync() > 0;
            }
        }

        public async Task<bool> RemoveDeliveryMethod(int id)
        {
            var entity = await _context.DeliveryMethods.FindAsync(id);
            var isInOrder = await _context.Orders.Where(x => x.DeliveryMethodId == id).FirstOrDefaultAsync();
            if (isInOrder != null)
            {
                return false;
            }
            else
            {
                _context.DeliveryMethods.Remove(entity);
                return await _context.SaveChangesAsync() > 0;
            }
        }

        public async Task<bool> RemovePromotion(int id)
        {
            var entity = await _context.Promotions.FindAsync(id);
            var isInProduct = await _context.Products.Where(x => x.PromotionId == id).FirstOrDefaultAsync();
            var isInOrder = await _context.OrderItems.Where(x => x.ItemOrdered.PromotionId == id).FirstOrDefaultAsync();
            if (isInProduct != null || isInOrder != null)
            {
                return false;
            }
            else
            {
                _context.Promotions.Remove(entity);
                return await _context.SaveChangesAsync() > 0;
            }
        }

        public async Task<bool> UpdateDeliveryMethod(DeliveryMethod deliveryMethod)
        {
            var isInOrder = await _context.Orders.Where(x => x.DeliveryMethodId == deliveryMethod.Id).FirstOrDefaultAsync();
            if (isInOrder != null)
            {
                return false;
            }
            else
            {
                _context.DeliveryMethods.Update(deliveryMethod);
                return await _context.SaveChangesAsync() > 0;
            }
        }
    }
}
