using System.ComponentModel.DataAnnotations;

namespace Acacia_Back_End.Core.ViewModels
{
    public class CustomerWishlistVM
    {
        [Required]
        public string Id { get; set; }

        public List<WishlistItemVM> Items { get; set; }
    }
}
