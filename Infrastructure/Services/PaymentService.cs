using Acacia_Back_End.Core.Interfaces;
using Acacia_Back_End.Core.Models;
using Acacia_Back_End.Infrastructure.Data;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.EntityFrameworkCore;
using Stripe;

namespace Acacia_Back_End.Infrastructure.Services
{
    public class PaymentService : IPaymentService
    {
        private readonly IBasketRepository _cartRepo;
        private readonly Context _context;
        private readonly IConfiguration _config;

        public PaymentService(IBasketRepository cartRepo, Context context, IConfiguration config)
        {
            _cartRepo = cartRepo;
            _context = context;
            _config = config;
        }
        public async Task<CustomerBasket> CreateOrUpdatePayment(string basketId)
        {
            StripeConfiguration.ApiKey = _config["StripeSettings:SecretKey"];

            var cart = await _cartRepo.GetBasketAsync(basketId);
            var shippingPrice = 0m;

            if (cart.DeliveryMethodId.HasValue)
            {
                var deliveryMethod = await _context.DeliveryMethods.Where(x => x.Id == cart.DeliveryMethodId).FirstOrDefaultAsync();
                shippingPrice = deliveryMethod.Price;
            }

            foreach (var item in cart.Items)
            {
                var productItem = await _context.Products.Where(x => x.Id == item.Id).FirstOrDefaultAsync();
                if (item.Price != productItem.GetPrice())
                {
                    item.Price = productItem.GetPrice();
                }
            }
            var service = new PaymentIntentService();

            PaymentIntent intent;

            if (string.IsNullOrEmpty(cart.PaymentIntentId))
            {
                var options = new PaymentIntentCreateOptions
                {
                    Amount = (long)cart.Items.Sum(i => i.Quantity * (i.Price * 100)) + (long)
                    shippingPrice * 100,
                    Currency = "usd",
                    PaymentMethodTypes = new List<string> { "card" }
                };
                intent = await service.CreateAsync(options);
                cart.PaymentIntentId = intent.Id;
                cart.ClientSecret = intent.ClientSecret;
            }
            else
            {
                var options = new PaymentIntentUpdateOptions
                {
                    Amount = (long)cart.Items.Sum(i => i.Quantity * (i.Price * 100)) + (long)
                    shippingPrice * 100,
                };
                await service.UpdateAsync(cart.PaymentIntentId, options);
            }

            await _cartRepo.UpdateBasketAsync(cart);

            return cart;
        }
    }
}
