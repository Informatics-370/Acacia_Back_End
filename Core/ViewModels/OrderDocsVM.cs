using System.ComponentModel.DataAnnotations;

namespace Acacia_Back_End.Core.ViewModels
{
    public class OrderDocsVM
    {
        [Required]
        public int SupplierId { get; set; }
        [Required]
        public IFormFile InvoiceUrl { get; set; }
        [Required]
        public IFormFile ProofOfPaymentUrl { get; set; }
    }
}
