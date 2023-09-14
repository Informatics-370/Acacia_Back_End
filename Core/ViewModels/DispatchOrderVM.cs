using System.ComponentModel.DataAnnotations;

namespace Acacia_Back_End.Core.ViewModels
{
    public class DispatchOrderVM
    {
        [Required]
        public int OrderId { get; set; }
        [Required]
        public string TrackingNumber { get; set; }
        [Required]
        public string CustomerEmail { get; set; }
    }
}
