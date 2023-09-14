using Acacia_Back_End.Core.Models;
using System.ComponentModel.DataAnnotations;

namespace Acacia_Back_End.Core.ViewModels
{
    public class CustomerCartVM
    {
        [Required]
        public string Id { get; set; }

        public List<CartItemVM> Items { get; set; }
        public int? DeliveryMethodId { get; set; }
        public string ClientSecret { get; set; }
        public string PaymentIntentId { get; set; }
        public decimal ShippingPrice { get; set; }
    }
}
