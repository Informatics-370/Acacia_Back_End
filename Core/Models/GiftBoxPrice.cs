using System.ComponentModel.DataAnnotations;

namespace Acacia_Back_End.Core.Models
{
    public class GiftBoxPrice:BaseEntity
    {
        public int GiftBoxId { get; set; }
        public decimal Price { get; set; }
        public decimal PackagingCosts { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }
    }
}
